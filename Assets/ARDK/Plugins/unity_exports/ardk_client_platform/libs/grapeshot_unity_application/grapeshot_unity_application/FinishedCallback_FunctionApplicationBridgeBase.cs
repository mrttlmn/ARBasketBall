//------------------------------------------------------------------------------
// <auto-generated />
//
// This file was automatically generated by SWIG (http://www.swig.org).
// Version 4.0.0
//
// Do not make changes to this file unless you know what you are doing--modify
// the SWIG interface file instead.
//------------------------------------------------------------------------------

namespace Grapeshot {

public abstract class FinishedCallback_FunctionApplicationBridgeBase : global::System.IDisposable {
  /** DO NOT REMOVE THE FOLLOWING COMMENT **/
  /** THIS HAS BEEN SWIG CORRECTED: JJDKFJSIIIIDKJ **/
  private global::System.Runtime.InteropServices.GCHandle _swigDelegateHandle1;
  private global::System.Runtime.InteropServices.GCHandle _swigDelegateHandle0;
  private global::System.Runtime.InteropServices.HandleRef swigCPtr;
  protected bool swigCMemOwn;

  internal FinishedCallback_FunctionApplicationBridgeBase(global::System.IntPtr cPtr, bool cMemoryOwn) {
    swigCMemOwn = cMemoryOwn;
    swigCPtr = new global::System.Runtime.InteropServices.HandleRef(this, cPtr);
  }

  internal static global::System.Runtime.InteropServices.HandleRef getCPtr(FinishedCallback_FunctionApplicationBridgeBase obj) {
    return (obj == null) ? new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero) : obj.swigCPtr;
  }

  ~FinishedCallback_FunctionApplicationBridgeBase() {
    Dispose();
  }

  public virtual void Dispose() {
    lock(this) {
      if (swigCPtr.Handle != global::System.IntPtr.Zero) {
        if (swigCMemOwn) {
          swigCMemOwn = false;
          GrapeshotSessionPINVOKE.delete_FinishedCallback_FunctionApplicationBridgeBase(swigCPtr);
        }
        swigCPtr = new global::System.Runtime.InteropServices.HandleRef(null, global::System.IntPtr.Zero);
      }
      global::System.GC.SuppressFinalize(this);
      _swigDelegateHandle0.Free();
      _swigDelegateHandle1.Free();
    }
  }

    // Information used to prevent pre-mature GC of callbacks still being used from C++.
    private ulong _refCount;
    private System.Runtime.InteropServices.GCHandle _handle;

    /// Allocates a reference to this application callback so that it can't be released from memory
    /// until all allocs have been meet with an equal number of releaseApplicationCallback.
    public void allocApplicationCallback() {
        lock(this) {
            if (_refCount == 0) {
                _handle = System.Runtime.InteropServices.GCHandle.Alloc(this);
            }

            _refCount++;
        }
    }

  public virtual void invokeApplicationLogic(bool FinishedCallback_arg) { throw new System.NotImplementedException(); }

  public virtual void releaseApplicationCallback() {
    // When releasing the application side callback, this means that one of the native side holds
    // was terminated, thus cs should remove a reference count, once that count is zero free the
    // handle.
    lock(this) {
        if (_refCount <= 0) {
            return;
        }

        _refCount -= 1;

        if (_refCount <= 0) {
            _handle.Free();
        }
    }
  }


  public FinishedCallback_FunctionApplicationBridgeBase() : this(GrapeshotSessionPINVOKE.new_FinishedCallback_FunctionApplicationBridgeBase(), true) {
    SwigDirectorConnect();
  }

  private void SwigDirectorConnect() {
    global::System.IntPtr swigDelegate0gcHandlePtr = global::System.IntPtr.Zero;
    if (SwigDerivedClassHasMethod("invokeApplicationLogic", swigMethodTypes0)) {
      swigDelegate0 = new SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_0(SwigDirectorinvokeApplicationLogic);
      swigDelegate0dispatcher = new SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_0_Dispatcher(SwigDirectorinvokeApplicationLogic_Dispatcher);
      
      global::System.Runtime.InteropServices.GCHandle swigDelegate0gcHandle = global::System.Runtime.InteropServices.GCHandle.Alloc(swigDelegate0, global::System.Runtime.InteropServices.GCHandleType.Normal);
      _swigDelegateHandle0 = swigDelegate0gcHandle;
      swigDelegate0gcHandlePtr = global::System.Runtime.InteropServices.GCHandle.ToIntPtr(swigDelegate0gcHandle);
    }
    global::System.IntPtr swigDelegate1gcHandlePtr = global::System.IntPtr.Zero;
    if (SwigDerivedClassHasMethod("releaseApplicationCallback", swigMethodTypes1)) {
      swigDelegate1 = new SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_1(SwigDirectorreleaseApplicationCallback);
      swigDelegate1dispatcher = new SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_1_Dispatcher(SwigDirectorreleaseApplicationCallback_Dispatcher);
      
      global::System.Runtime.InteropServices.GCHandle swigDelegate1gcHandle = global::System.Runtime.InteropServices.GCHandle.Alloc(swigDelegate1, global::System.Runtime.InteropServices.GCHandleType.Normal);
      _swigDelegateHandle1 = swigDelegate1gcHandle;
      swigDelegate1gcHandlePtr = global::System.Runtime.InteropServices.GCHandle.ToIntPtr(swigDelegate1gcHandle);
    }
    GrapeshotSessionPINVOKE.FinishedCallback_FunctionApplicationBridgeBase_director_connect(swigCPtr, swigDelegate0dispatcher, swigDelegate0gcHandlePtr, swigDelegate1dispatcher, swigDelegate1gcHandlePtr);
  }

  private bool SwigDerivedClassHasMethod(string methodName, global::System.Type[] methodTypes) {
    global::System.Reflection.MethodInfo methodInfo = this.GetType().GetMethod(methodName, global::System.Reflection.BindingFlags.Public | global::System.Reflection.BindingFlags.NonPublic | global::System.Reflection.BindingFlags.Instance, null, methodTypes, null);
    bool hasDerivedMethod = methodInfo.DeclaringType.IsSubclassOf(typeof(FinishedCallback_FunctionApplicationBridgeBase));
    return hasDerivedMethod;
  }

  private void SwigDirectorinvokeApplicationLogic(bool FinishedCallback_arg) {
    invokeApplicationLogic(FinishedCallback_arg);
  }

  [GrapeshotSessionPINVOKE.MonoPInvokeCallback(typeof(SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_0_Dispatcher))]
  private static void SwigDirectorinvokeApplicationLogic_Dispatcher(global::System.IntPtr swigDelegateFinishedCallback_FunctionApplicationBridgeBase_0_Handle, bool FinishedCallback_arg) {
    global::System.Runtime.InteropServices.GCHandle gcHandle = global::System.Runtime.InteropServices.GCHandle.FromIntPtr(swigDelegateFinishedCallback_FunctionApplicationBridgeBase_0_Handle);
    SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_0 delegateSwigDelegateFinishedCallback_FunctionApplicationBridgeBase_0 = (SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_0) gcHandle.Target;
delegateSwigDelegateFinishedCallback_FunctionApplicationBridgeBase_0(FinishedCallback_arg);
  }

  private void SwigDirectorreleaseApplicationCallback() {
    releaseApplicationCallback();
  }

  [GrapeshotSessionPINVOKE.MonoPInvokeCallback(typeof(SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_1_Dispatcher))]
  private static void SwigDirectorreleaseApplicationCallback_Dispatcher(global::System.IntPtr swigDelegateFinishedCallback_FunctionApplicationBridgeBase_1_Handle) {
    global::System.Runtime.InteropServices.GCHandle gcHandle = global::System.Runtime.InteropServices.GCHandle.FromIntPtr(swigDelegateFinishedCallback_FunctionApplicationBridgeBase_1_Handle);
    SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_1 delegateSwigDelegateFinishedCallback_FunctionApplicationBridgeBase_1 = (SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_1) gcHandle.Target;
delegateSwigDelegateFinishedCallback_FunctionApplicationBridgeBase_1();
  }

  public delegate void SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_0(bool FinishedCallback_arg);
  public delegate void SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_1();

  private SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_0 swigDelegate0;
  private SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_1 swigDelegate1;

  public delegate void SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_0_Dispatcher(global::System.IntPtr swigDelegateFinishedCallback_FunctionApplicationBridgeBase_0_Handle, bool FinishedCallback_arg);
  public delegate void SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_1_Dispatcher(global::System.IntPtr swigDelegateFinishedCallback_FunctionApplicationBridgeBase_1_Handle);

  private SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_0_Dispatcher swigDelegate0dispatcher;
  private SwigDelegateFinishedCallback_FunctionApplicationBridgeBase_1_Dispatcher swigDelegate1dispatcher;

  private static global::System.Type[] swigMethodTypes0 = new global::System.Type[] { typeof(bool) };
  private static global::System.Type[] swigMethodTypes1 = new global::System.Type[] {  };
}

}
