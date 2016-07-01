namespace OPCDA.NET
{
    using System;
    using System.Collections;
    using System.Threading;

    internal class ItemCollection : CollectionBase
    {
        private Mutex mtx = new Mutex();

        internal void Add(ItemDef gItem)
        {
            this.Lock();
            base.List.Add(gItem);
            this.Unlock();
        }

        internal bool Contains(string name)
        {
            this.Lock();
            IEnumerator enumerator = base.List.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ItemDef current = (ItemDef) enumerator.Current;
                if (name == current.OpcIDef.ItemID)
                {
                    this.Unlock();
                    return true;
                }
            }
            this.Unlock();
            return false;
        }

        public ItemDef FindClientHandle(int Hnd)
        {
            this.Lock();
            IEnumerator enumerator = base.List.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ItemDef current = (ItemDef) enumerator.Current;
                if (Hnd == current.OpcIDef.HandleClient)
                {
                    this.Unlock();
                    return current;
                }
            }
            this.Unlock();
            return null;
        }

        internal int IndexOf(string name)
        {
            this.Lock();
            for (int i = 0; i < base.List.Count; i++)
            {
                if (base.List[i] != null)
                {
                    ItemDef def = (ItemDef) base.List[i];
                    if (name == def.OpcIDef.ItemID)
                    {
                        this.Unlock();
                        return i;
                    }
                }
            }
            this.Unlock();
            return -1;
        }

        internal ItemDef Item(int Index)
        {
            this.Lock();
            ItemDef def = (ItemDef) base.List[Index];
            this.Unlock();
            return def;
        }

        public ItemDef Item(string name)
        {
            this.Lock();
            IEnumerator enumerator = base.List.GetEnumerator();
            while (enumerator.MoveNext())
            {
                ItemDef current = (ItemDef) enumerator.Current;
                if (name == current.OpcIDef.ItemID)
                {
                    this.Unlock();
                    return current;
                }
            }
            this.Unlock();
            return null;
        }

        private void Lock()
        {
            this.mtx.WaitOne();
        }

        internal void Remove(ItemDef gItem)
        {
            this.Lock();
            base.List.Remove(gItem);
            this.Unlock();
        }

        private void Unlock()
        {
            this.mtx.ReleaseMutex();
        }
    }
}

