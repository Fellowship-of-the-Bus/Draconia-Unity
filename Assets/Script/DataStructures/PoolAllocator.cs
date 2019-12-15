using System;
using System.Diagnostics;               // for assert, to avoid unity dependencies
using System.Runtime.InteropServices;

namespace Draconia.Collections.StaticAlloc {
  internal unsafe struct PoolNode {
    public PoolNode * next;
  }
  
  // Simple pool allocator for a fixed number of elements of an unmanaged type T.
  // Typically for internal use in allocating nodes for StaticAlloc data structures
  public unsafe class PoolAllocator<T> : IDisposable where T : unmanaged {
    private IntPtr mem;
    private PoolNode * freeList;

    public PoolAllocator(int size) {
      int elemSize = Math.Max(sizeof(T), sizeof(PoolNode));
      mem = Marshal.AllocHGlobal(size*elemSize);
      for (byte * ptr = (byte *)mem, end = (byte *)mem + size * elemSize; ptr < end; ptr += elemSize) {
        PoolNode * node = (PoolNode *)ptr;
        node->next = freeList;
        freeList = node;
      }
    }

    ~PoolAllocator() {
      CleanUp();
    }

    void IDisposable.Dispose() {
      CleanUp();
      GC.SuppressFinalize(this);
    }

    private void CleanUp() {
      Marshal.FreeHGlobal(mem);
      mem = new IntPtr();
    }

    public T * Alloc() {
      Debug.Assert(freeList != null, "trying to add an element to a full list");
      PoolNode * ret = freeList;
      freeList = freeList->next;
      return (T *)ret;
    }

    public void Free(T * ptr) {
      PoolNode * node = (PoolNode *)ptr;
      node->next = freeList;
      freeList = node;
    }
  }
}