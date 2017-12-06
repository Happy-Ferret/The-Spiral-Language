module SpiralExample.Main
let cuda_kernels = """

extern "C" {
    __global__ void method_8(float var_0, float * var_1, float * var_2);
    __device__ void method_9(float var_0, float * var_1, float * var_2, long long int var_3);
    
    __global__ void method_8(float var_0, float * var_1, float * var_2) {
        long long int var_3 = threadIdx.x;
        long long int var_4 = threadIdx.y;
        long long int var_5 = threadIdx.z;
        long long int var_6 = blockIdx.x;
        long long int var_7 = blockIdx.y;
        long long int var_8 = blockIdx.z;
        long long int var_9 = (var_6 * 128);
        long long int var_10 = (var_9 + var_3);
        method_9(var_0, var_1, var_2, var_10);
    }
    __device__ void method_9(float var_0, float * var_1, float * var_2, long long int var_3) {
        char var_4 = (var_3 < 32);
        if (var_4) {
            char var_5 = (var_3 >= 0);
            char var_6 = (var_5 == 0);
            if (var_6) {
                // unprinted assert;
            } else {
            }
            float * var_7 = var_2 + var_3;
            if (var_6) {
                // unprinted assert;
            } else {
            }
            float * var_8 = var_1 + var_3;
            float var_9 = var_8[0];
            float var_10 = (var_0 * var_9);
            var_7[0] = var_10;
            long long int var_11 = (var_3 + 4096);
            method_9(var_0, var_1, var_2, var_11);
        } else {
        }
    }
}
"""

type Union0 =
    | Union0Case0 of Tuple2
    | Union0Case1
and Tuple1 =
    struct
    val mem_0: ManagedCuda.BasicTypes.CUdeviceptr
    new(arg_mem_0) = {mem_0 = arg_mem_0}
    end
and Tuple2 =
    struct
    val mem_0: Tuple1
    new(arg_mem_0) = {mem_0 = arg_mem_0}
    end
and EnvStack3 =
    struct
    val mem_0: (Union0 ref)
    new(arg_mem_0) = {mem_0 = arg_mem_0}
    end
and Env4 =
    struct
    val mem_0: EnvStack3
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_0 ((var_0: System.Diagnostics.DataReceivedEventArgs)): unit =
    let (var_1: string) = var_0.get_Data()
    System.Console.WriteLine(var_1)
and method_1((var_0: (Union0 ref))): ManagedCuda.BasicTypes.CUdeviceptr =
    let (var_1: Union0) = (!var_0)
    match var_1 with
    | Union0Case0(var_2) ->
        let (var_3: Tuple1) = var_2.mem_0
        var_3.mem_0
    | Union0Case1 ->
        (failwith "A Cuda memory cell that has been disposed has been tried to be accessed.")
and method_2((var_0: (float32 [])), (var_1: int64)): unit =
    let (var_2: bool) = (var_1 < 32L)
    if var_2 then
        let (var_3: bool) = (var_1 >= 0L)
        let (var_4: bool) = (var_3 = false)
        if var_4 then
            (failwith "Argument out of bounds.")
        else
            ()
        let (var_5: float32) = (float32 var_1)
        var_0.[int32 var_1] <- var_5
        let (var_6: int64) = (var_1 + 1L)
        method_2((var_0: (float32 [])), (var_6: int64))
    else
        ()
and method_3((var_0: (float32 [])), (var_1: int64), (var_2: int64)): unit =
    let (var_3: bool) = (var_2 < var_1)
    if var_3 then
        let (var_4: bool) = (var_2 >= 0L)
        let (var_5: bool) = (var_4 = false)
        if var_5 then
            (failwith "Argument out of bounds.")
        else
            ()
        let (var_6: float32) = (float32 var_2)
        var_0.[int32 var_2] <- var_6
        let (var_7: int64) = (var_2 + 1L)
        method_3((var_0: (float32 [])), (var_1: int64), (var_7: int64))
    else
        ()
and method_4((var_0: uint64), (var_1: System.Collections.Generic.Stack<Env4>), (var_2: uint64), (var_3: int64)): EnvStack3 =
    let (var_4: int32) = var_1.get_Count()
    let (var_5: bool) = (var_4 > 0)
    if var_5 then
        let (var_6: Env4) = var_1.Peek()
        let (var_7: EnvStack3) = var_6.mem_0
        let (var_8: int64) = var_6.mem_1
        let (var_9: (Union0 ref)) = var_7.mem_0
        let (var_10: Union0) = (!var_9)
        match var_10 with
        | Union0Case0(var_11) ->
            let (var_12: Tuple1) = var_11.mem_0
            let (var_13: ManagedCuda.BasicTypes.CUdeviceptr) = var_12.mem_0
            method_5((var_13: ManagedCuda.BasicTypes.CUdeviceptr), (var_0: uint64), (var_2: uint64), (var_3: int64), (var_1: System.Collections.Generic.Stack<Env4>), (var_7: EnvStack3), (var_8: int64))
        | Union0Case1 ->
            let (var_15: Env4) = var_1.Pop()
            let (var_16: EnvStack3) = var_15.mem_0
            let (var_17: int64) = var_15.mem_1
            method_4((var_0: uint64), (var_1: System.Collections.Generic.Stack<Env4>), (var_2: uint64), (var_3: int64))
    else
        method_6((var_0: uint64), (var_1: System.Collections.Generic.Stack<Env4>), (var_2: uint64), (var_3: int64))
and method_7((var_0: ManagedCuda.BasicTypes.CUdeviceptr)): ManagedCuda.BasicTypes.CUdeviceptr =
    let (var_1: ManagedCuda.BasicTypes.SizeT) = var_0.Pointer
    let (var_2: uint64) = uint64 var_1
    let (var_3: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(var_2)
    let (var_4: ManagedCuda.BasicTypes.CUdeviceptr) = ManagedCuda.BasicTypes.CUdeviceptr(var_3)
    var_4
and method_10((var_0: System.Text.StringBuilder), (var_1: int64)): unit =
    let (var_2: bool) = (var_1 < 0L)
    if var_2 then
        let (var_3: System.Text.StringBuilder) = var_0.Append(' ')
        let (var_4: int64) = (var_1 + 1L)
        method_10((var_0: System.Text.StringBuilder), (var_4: int64))
    else
        ()
and method_11((var_0: System.Text.StringBuilder), (var_1: (float32 [])), (var_2: int64), (var_3: string)): string =
    let (var_4: bool) = (var_2 < 32L)
    if var_4 then
        let (var_5: System.Text.StringBuilder) = var_0.Append(var_3)
        let (var_6: bool) = (var_2 >= 0L)
        let (var_7: bool) = (var_6 = false)
        if var_7 then
            (failwith "Argument out of bounds.")
        else
            ()
        let (var_8: float32) = var_1.[int32 var_2]
        let (var_9: string) = System.String.Format("{0}",var_8)
        let (var_10: System.Text.StringBuilder) = var_0.Append(var_9)
        let (var_11: string) = "; "
        let (var_12: int64) = (var_2 + 1L)
        method_11((var_0: System.Text.StringBuilder), (var_1: (float32 [])), (var_12: int64), (var_11: string))
    else
        var_3
and method_5((var_0: ManagedCuda.BasicTypes.CUdeviceptr), (var_1: uint64), (var_2: uint64), (var_3: int64), (var_4: System.Collections.Generic.Stack<Env4>), (var_5: EnvStack3), (var_6: int64)): EnvStack3 =
    let (var_7: ManagedCuda.BasicTypes.SizeT) = var_0.Pointer
    let (var_8: uint64) = uint64 var_7
    let (var_9: uint64) = uint64 var_6
    let (var_10: uint64) = (var_8 - var_1)
    let (var_11: uint64) = (var_10 + var_9)
    let (var_12: uint64) = uint64 var_3
    let (var_13: uint64) = (var_12 + var_11)
    let (var_14: bool) = (var_13 <= var_2)
    let (var_15: bool) = (var_14 = false)
    if var_15 then
        (failwith "Cache size has been exceeded in the allocator.")
    else
        ()
    let (var_16: uint64) = (var_8 + var_9)
    let (var_17: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(var_16)
    let (var_18: ManagedCuda.BasicTypes.CUdeviceptr) = ManagedCuda.BasicTypes.CUdeviceptr(var_17)
    let (var_19: (Union0 ref)) = (ref (Union0Case0(Tuple2(Tuple1(var_18)))))
    let (var_20: EnvStack3) = EnvStack3((var_19: (Union0 ref)))
    var_4.Push((Env4(var_20, var_3)))
    var_20
and method_6((var_0: uint64), (var_1: System.Collections.Generic.Stack<Env4>), (var_2: uint64), (var_3: int64)): EnvStack3 =
    let (var_4: uint64) = uint64 var_3
    let (var_5: bool) = (var_4 <= var_2)
    let (var_6: bool) = (var_5 = false)
    if var_6 then
        (failwith "Cache size has been exceeded in the allocator.")
    else
        ()
    let (var_7: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(var_0)
    let (var_8: ManagedCuda.BasicTypes.CUdeviceptr) = ManagedCuda.BasicTypes.CUdeviceptr(var_7)
    let (var_9: (Union0 ref)) = (ref (Union0Case0(Tuple2(Tuple1(var_8)))))
    let (var_10: EnvStack3) = EnvStack3((var_9: (Union0 ref)))
    var_1.Push((Env4(var_10, var_3)))
    var_10
let (var_0: string) = cuda_kernels
let (var_1: ManagedCuda.CudaContext) = ManagedCuda.CudaContext(false)
var_1.Synchronize()
let (var_2: string) = System.Environment.get_CurrentDirectory()
let (var_3: string) = System.IO.Path.Combine(var_2, "nvcc_router.bat")
let (var_4: System.Diagnostics.ProcessStartInfo) = System.Diagnostics.ProcessStartInfo()
var_4.set_RedirectStandardOutput(true)
var_4.set_RedirectStandardError(true)
var_4.set_UseShellExecute(false)
var_4.set_FileName(var_3)
let (var_5: System.Diagnostics.Process) = System.Diagnostics.Process()
var_5.set_StartInfo(var_4)
let (var_7: (System.Diagnostics.DataReceivedEventArgs -> unit)) = method_0
var_5.OutputDataReceived.Add(var_7)
var_5.ErrorDataReceived.Add(var_7)
let (var_8: string) = System.IO.Path.Combine("C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Community", "VC\\Auxiliary\\Build\\vcvars64.bat")
let (var_9: string) = System.IO.Path.Combine("C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Community", "VC\\Tools\\MSVC\\14.11.25503\\bin\\Hostx64\\x64")
let (var_10: string) = System.IO.Path.Combine("C:\\Program Files\\NVIDIA GPU Computing Toolkit\\CUDA\\v9.0", "include")
let (var_11: string) = System.IO.Path.Combine("C:\\Program Files (x86)\\Microsoft Visual Studio\\2017\\Community", "VC\\Tools\\MSVC\\14.11.25503\\include")
let (var_12: string) = System.IO.Path.Combine("C:\\Program Files\\NVIDIA GPU Computing Toolkit\\CUDA\\v9.0", "bin\\nvcc.exe")
let (var_13: string) = System.IO.Path.Combine(var_2, "cuda_kernels.ptx")
let (var_14: string) = System.IO.Path.Combine(var_2, "cuda_kernels.cu")
let (var_15: bool) = System.IO.File.Exists(var_14)
if var_15 then
    System.IO.File.Delete(var_14)
else
    ()
System.IO.File.WriteAllText(var_14, var_0)
let (var_16: bool) = System.IO.File.Exists(var_3)
if var_16 then
    System.IO.File.Delete(var_3)
else
    ()
let (var_17: System.IO.FileStream) = System.IO.File.OpenWrite(var_3)
let (var_18: System.IO.StreamWriter) = System.IO.StreamWriter(var_17)
var_18.WriteLine("SETLOCAL")
let (var_19: string) = String.concat "" [|"CALL "; "\""; var_8; "\""|]
var_18.WriteLine(var_19)
let (var_20: string) = String.concat "" [|"SET PATH=%PATH%;"; "\""; var_9; "\""|]
var_18.WriteLine(var_20)
let (var_21: string) = String.concat "" [|"\""; var_12; "\" -gencode=arch=compute_30,code=\\\"sm_30,compute_30\\\" --use-local-env --cl-version 2017 -I\""; var_10; "\" -I\"C:\\cub-1.7.4\" -I\""; var_11; "\" --keep-dir \""; var_2; "\" -maxrregcount=0  --machine 64 -ptx -cudart static  -o \""; var_13; "\" \""; var_14; "\""|]
var_18.WriteLine(var_21)
var_18.Dispose()
var_17.Dispose()
let (var_22: System.Diagnostics.Stopwatch) = System.Diagnostics.Stopwatch.StartNew()
let (var_23: bool) = var_5.Start()
let (var_24: bool) = (var_23 = false)
if var_24 then
    (failwith "NVCC failed to run.")
else
    ()
var_5.BeginOutputReadLine()
var_5.BeginErrorReadLine()
var_5.WaitForExit()
let (var_25: int32) = var_5.get_ExitCode()
let (var_26: bool) = (var_25 = 0)
let (var_27: bool) = (var_26 = false)
if var_27 then
    let (var_28: string) = System.String.Format("{0}",var_25)
    let (var_29: string) = System.String.Format("[{1}]","NVCC failed compilation.",var_28)
    (failwith var_29)
else
    ()
let (var_30: System.TimeSpan) = var_22.get_Elapsed()
printfn "The time it took to compile the Cuda kernels is: %A" var_30
let (var_31: ManagedCuda.BasicTypes.CUmodule) = var_1.LoadModulePTX(var_13)
var_5.Dispose()
let (var_32: string) = String.concat "" [|"Compiled the kernels into the following directory: "; var_2|]
System.Console.WriteLine(var_32)
let (var_33: ManagedCuda.CudaDeviceProperties) = var_1.GetDeviceInfo()
let (var_34: ManagedCuda.BasicTypes.SizeT) = var_33.get_TotalGlobalMemory()
let (var_35: int64) = int64 var_34
let (var_36: float) = float var_35
let (var_37: float) = (0.700000 * var_36)
let (var_38: int64) = int64 var_37
let (var_39: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(var_38)
let (var_40: ManagedCuda.BasicTypes.CUdeviceptr) = var_1.AllocateMemory(var_39)
let (var_41: (Union0 ref)) = (ref (Union0Case0(Tuple2(Tuple1(var_40)))))
let (var_42: EnvStack3) = EnvStack3((var_41: (Union0 ref)))
let (var_43: System.Collections.Generic.Stack<Env4>) = System.Collections.Generic.Stack<Env4>()
let (var_44: (Union0 ref)) = var_42.mem_0
let (var_45: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_44: (Union0 ref)))
let (var_46: ManagedCuda.BasicTypes.SizeT) = var_45.Pointer
let (var_47: uint64) = uint64 var_46
let (var_48: uint64) = uint64 var_38
let (var_49: ManagedCuda.CudaRand.GeneratorType) = ManagedCuda.CudaRand.GeneratorType.PseudoDefault
let (var_50: ManagedCuda.CudaRand.CudaRandDevice) = ManagedCuda.CudaRand.CudaRandDevice(var_49)
let (var_51: ManagedCuda.CudaBlas.PointerMode) = ManagedCuda.CudaBlas.PointerMode.Host
let (var_52: ManagedCuda.CudaBlas.AtomicsMode) = ManagedCuda.CudaBlas.AtomicsMode.Allowed
let (var_53: ManagedCuda.CudaBlas.CudaBlas) = ManagedCuda.CudaBlas.CudaBlas(var_51, var_52)
let (var_54: ManagedCuda.CudaStream) = ManagedCuda.CudaStream()
let (var_55: (float32 [])) = Array.zeroCreate<float32> (System.Convert.ToInt32(32L))
let (var_56: int64) = 0L
method_2((var_55: (float32 [])), (var_56: int64))
let (var_57: float32) = 2.000000f
let (var_58: int64) = 64L
let (var_59: bool) = (var_58 > 0L)
let (var_60: bool) = (var_59 = false)
if var_60 then
    (failwith "Tensor needs to be at least size 1.")
else
    ()
let (var_61: (float32 [])) = Array.zeroCreate<float32> (System.Convert.ToInt32(var_58))
let (var_62: int64) = 0L
method_3((var_61: (float32 [])), (var_58: int64), (var_62: int64))
let (var_63: float32) = sqrt 912.000000f
let (var_64: float32) = (1.000000f / var_63)
let (var_65: int64) = var_55.LongLength
let (var_66: int64) = (int64 sizeof<float32>)
let (var_67: int64) = (var_65 * var_66)
let (var_68: EnvStack3) = method_4((var_47: uint64), (var_43: System.Collections.Generic.Stack<Env4>), (var_48: uint64), (var_67: int64))
let (var_69: (Union0 ref)) = var_68.mem_0
let (var_70: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_69: (Union0 ref)))
var_1.CopyToDevice(var_70, var_55)
let (var_72: int64) = (32L * var_66)
let (var_73: EnvStack3) = method_4((var_47: uint64), (var_43: System.Collections.Generic.Stack<Env4>), (var_48: uint64), (var_72: int64))
let (var_74: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_69: (Union0 ref)))
let (var_75: ManagedCuda.BasicTypes.CUdeviceptr) = method_7((var_74: ManagedCuda.BasicTypes.CUdeviceptr))
let (var_76: (Union0 ref)) = var_73.mem_0
let (var_77: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_76: (Union0 ref)))
let (var_78: ManagedCuda.BasicTypes.CUdeviceptr) = method_7((var_77: ManagedCuda.BasicTypes.CUdeviceptr))
// Cuda join point
// method_8((var_57: float32), (var_75: ManagedCuda.BasicTypes.CUdeviceptr), (var_78: ManagedCuda.BasicTypes.CUdeviceptr))
let (var_79: (System.Object [])) = Array.zeroCreate<System.Object> (System.Convert.ToInt32(3L))
var_79.[int32 0L] <- (var_57 :> System.Object)
var_79.[int32 1L] <- (var_75 :> System.Object)
var_79.[int32 2L] <- (var_78 :> System.Object)
let (var_80: ManagedCuda.CudaKernel) = ManagedCuda.CudaKernel("method_8", var_31, var_1)
let (var_81: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(32u, 1u, 1u)
var_80.set_GridDimensions(var_81)
let (var_82: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(128u, 1u, 1u)
var_80.set_BlockDimensions(var_82)
let (var_83: ManagedCuda.BasicTypes.CUstream) = var_54.get_Stream()
var_80.RunAsync(var_83, var_79)
let (var_84: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_76: (Union0 ref)))
let (var_85: (float32 [])) = Array.zeroCreate<float32> (System.Convert.ToInt32(32L))
var_1.CopyToHost(var_85, var_84)
var_1.Synchronize()
let (var_86: System.Text.StringBuilder) = System.Text.StringBuilder()
let (var_87: string) = ""
let (var_88: int64) = 0L
method_10((var_86: System.Text.StringBuilder), (var_88: int64))
let (var_89: System.Text.StringBuilder) = var_86.Append("[|")
let (var_90: int64) = 0L
let (var_91: string) = method_11((var_86: System.Text.StringBuilder), (var_85: (float32 [])), (var_90: int64), (var_87: string))
let (var_92: System.Text.StringBuilder) = var_86.AppendLine("|]")
let (var_93: string) = var_86.ToString()
System.Console.WriteLine(var_93)
var_76 := Union0Case1
var_69 := Union0Case1
var_53.Dispose()
var_50.Dispose()
let (var_94: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_44: (Union0 ref)))
var_1.FreeMemory(var_94)
var_44 := Union0Case1
var_1.Dispose()

