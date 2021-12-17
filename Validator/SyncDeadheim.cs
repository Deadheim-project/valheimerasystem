using HarmonyLib;
using System;

namespace Deadheim
{

    public static class SyncDeadheim
    {
        public static void RPC_VersionValidator(ZRpc rpc, ZPackage pkg)
        {
            if (!ZNet.instance.IsServer()) return;

            var version = pkg.ReadString();
            if (version != Plugin.Version)
            {
                rpc.Invoke("Error", (object)3);
            }
            else
            {
                Plugin.validatedUsers.Add(rpc);
            }
        }

        [HarmonyPatch(typeof(ZNet), "OnNewConnection")]
        public static class OnNewConnection
        {
            private static void Prefix(ZNetPeer peer, ref ZNet __instance)
            {
                peer.m_rpc.Register<ZPackage>("VersionValidator", new Action<ZRpc, ZPackage>(RPC_VersionValidator));
                ZPackage zpackage = new ZPackage();
                zpackage.Write(Plugin.Version);
                peer.m_rpc.Invoke("VersionValidator", zpackage);
            }
        }

        [HarmonyPatch(typeof(ZNet), "RPC_PeerInfo")]
        public static class RPC_PeerInfo
        {
            private static bool Prefix(ZRpc rpc, ref ZNet __instance)
            {
                if (!__instance.IsServer()) return true;

                if (!Plugin.validatedUsers.Contains(rpc))
                {
                    rpc.Invoke("Error", 3);
                    return false;
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(ZNet), "Disconnect")]
        public static class Disconnect
        {
            private static void Prefix(ZNetPeer peer, ref ZNet __instance)
            {
                if (__instance.IsServer()) return;

                Plugin.validatedUsers.Remove(peer.m_rpc);
            }
        }
    }
}