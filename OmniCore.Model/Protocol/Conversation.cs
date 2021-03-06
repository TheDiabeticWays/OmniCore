﻿using OmniCore.Model.Protocol.Base;
using System;
using System.Threading.Tasks;

namespace OmniCore.Model.Protocol
{
    public class Conversation : IDisposable
    {
        //private static object ConversationLock = new object();
        private IMessageRadio Radio;
        private Pod Pod;

        private Conversation(IMessageRadio radio, Pod pod)
        {
            this.Radio = radio;
            this.Pod = pod;
        }

        public static Conversation Start(IMessageRadio radio, Pod pod)
        {
            if (!pod.Address.HasValue)
                throw new ArgumentException("Pod address is unknown");

            //Monitor.Wait(ConversationLock);
            if (!radio.IsInitialized())
            {
                radio.InitializeRadio(pod.Address.Value);
                radio.SetNonceParameters(pod.Lot.Value, pod.Tid.Value, null, null);
            }
            return new Conversation(radio, pod);
        }

        public async Task<IMessage> SendRequest(IMessage request)
        {
            return await this.Radio.SendRequestAndGetResponse(request);
        }

        public void End()
        {
            //Monitor.Exit(ConversationLock);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    this.End();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~Conversation() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
