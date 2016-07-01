namespace OPCDA.NET
{
    using System;
    using System.Collections;
    using System.Threading;

    internal class Subscriptions : CollectionBase
    {
        private Mutex mtx = new Mutex();

        internal void Add(Subscription sItem)
        {
            this.Lock();
            base.List.Add(sItem);
            this.Unlock();
        }

        internal Subscription FindHandle(int hnd)
        {
            this.Lock();
            for (int i = 0; i < base.Count; i++)
            {
                Subscription subscription = (Subscription) base.List[i];
                if (subscription.clientHandle == hnd)
                {
                    this.Unlock();
                    return subscription;
                }
            }
            this.Unlock();
            return null;
        }

        internal Subscription Item(int Index)
        {
            this.Lock();
            Subscription subscription = (Subscription) base.List[Index];
            this.Unlock();
            return subscription;
        }

        private void Lock()
        {
            this.mtx.WaitOne();
        }

        internal void Remove(Subscription sItem)
        {
            this.Lock();
            base.List.Remove(sItem);
            this.Unlock();
        }

        private void Unlock()
        {
            this.mtx.ReleaseMutex();
        }
    }
}

