module SpiralExample.Main
let cuda_kernels = """
#include "cub/cub.cuh"

extern "C" {
    __global__ void method_6(float * var_0, float * var_1, float * var_2);
    __global__ void method_9(float * var_0, float * var_1);
    __global__ void method_11(float * var_0, float * var_1, float * var_2);
    __global__ void method_14(float var_0, float var_1, float * var_2, float * var_3, float * var_4);
    __global__ void method_15(float * var_0, float * var_1, float * var_2, float * var_3);
    __global__ void method_17(float * var_0, float * var_1);
    __device__ char method_7(long long int * var_0);
    __device__ char method_8(long long int * var_0);
    __device__ char method_10(long long int * var_0);
    __device__ char method_12(long long int * var_0, float * var_1);
    __device__ char method_18(long long int * var_0, float * var_1);
    __device__ char method_19(long long int * var_0, float * var_1);
    __device__ char method_20(long long int var_0, long long int * var_1, float * var_2);
    
    __global__ void method_6(float * var_0, float * var_1, float * var_2) {
        long long int var_3 = threadIdx.x;
        long long int var_4 = blockIdx.x;
        long long int var_5 = (4 * var_4);
        long long int var_6 = (var_3 + var_5);
        long long int var_7[1];
        var_7[0] = var_6;
        while (method_7(var_7)) {
            long long int var_9 = var_7[0];
            long long int var_10 = (var_9 + 4);
            char var_11 = (var_9 >= 0);
            char var_13;
            if (var_11) {
                var_13 = (var_9 < 4);
            } else {
                var_13 = 0;
            }
            char var_14 = (var_13 == 0);
            if (var_14) {
                // "Argument out of bounds."
            } else {
            }
            long long int var_15 = threadIdx.y;
            long long int var_16 = blockIdx.y;
            long long int var_17 = (2 * var_16);
            long long int var_18 = (var_15 + var_17);
            long long int var_19[1];
            var_19[0] = var_18;
            while (method_8(var_19)) {
                long long int var_21 = var_19[0];
                long long int var_22 = (var_21 + 2);
                char var_23 = (var_21 >= 0);
                char var_25;
                if (var_23) {
                    var_25 = (var_21 < 2);
                } else {
                    var_25 = 0;
                }
                char var_26 = (var_25 == 0);
                if (var_26) {
                    // "Argument out of bounds."
                } else {
                }
                long long int var_27 = (var_21 * 4);
                char var_29;
                if (var_11) {
                    var_29 = (var_9 < 4);
                } else {
                    var_29 = 0;
                }
                char var_30 = (var_29 == 0);
                if (var_30) {
                    // "Argument out of bounds."
                } else {
                }
                long long int var_31 = (var_27 + var_9);
                char var_33;
                if (var_23) {
                    var_33 = (var_21 < 2);
                } else {
                    var_33 = 0;
                }
                char var_34 = (var_33 == 0);
                if (var_34) {
                    // "Argument out of bounds."
                } else {
                }
                char var_36;
                if (var_11) {
                    var_36 = (var_9 < 4);
                } else {
                    var_36 = 0;
                }
                char var_37 = (var_36 == 0);
                if (var_37) {
                    // "Argument out of bounds."
                } else {
                }
                float var_38 = var_0[var_9];
                float var_39 = var_1[var_31];
                float var_40 = var_2[var_31];
                float var_41 = (var_38 + var_39);
                var_2[var_31] = var_41;
                var_19[0] = var_22;
            }
            long long int var_42 = var_19[0];
            var_7[0] = var_10;
        }
        long long int var_43 = var_7[0];
    }
    __global__ void method_9(float * var_0, float * var_1) {
        long long int var_2 = blockIdx.x;
        long long int var_3 = (var_2 * 128);
        long long int var_4 = threadIdx.x;
        long long int var_5 = (var_3 + var_4);
        long long int var_6[1];
        var_6[0] = var_5;
        while (method_10(var_6)) {
            long long int var_8 = var_6[0];
            long long int var_9 = (var_8 + 128);
            char var_10 = (var_8 >= 0);
            char var_12;
            if (var_10) {
                var_12 = (var_8 < 8);
            } else {
                var_12 = 0;
            }
            char var_13 = (var_12 == 0);
            if (var_13) {
                // "Argument out of bounds."
            } else {
            }
            char var_15;
            if (var_10) {
                var_15 = (var_8 < 8);
            } else {
                var_15 = 0;
            }
            char var_16 = (var_15 == 0);
            if (var_16) {
                // "Argument out of bounds."
            } else {
            }
            float var_17 = var_0[var_8];
            float var_18 = var_1[var_8];
            float var_19 = (-var_17);
            float var_20 = exp(var_19);
            float var_21 = (1 + var_20);
            float var_22 = (1 / var_21);
            var_1[var_8] = var_22;
            var_6[0] = var_9;
        }
        long long int var_23 = var_6[0];
    }
    __global__ void method_11(float * var_0, float * var_1, float * var_2) {
        long long int var_3 = blockIdx.x;
        long long int var_4 = (var_3 * 128);
        long long int var_5 = threadIdx.x;
        long long int var_6 = (var_4 + var_5);
        float var_7 = 0;
        long long int var_8[1];
        float var_9[1];
        var_8[0] = var_6;
        var_9[0] = var_7;
        while (method_12(var_8, var_9)) {
            long long int var_11 = var_8[0];
            float var_12 = var_9[0];
            long long int var_13 = (var_11 + 128);
            char var_14 = (var_11 >= 0);
            char var_16;
            if (var_14) {
                var_16 = (var_11 < 8);
            } else {
                var_16 = 0;
            }
            char var_17 = (var_16 == 0);
            if (var_17) {
                // "Argument out of bounds."
            } else {
            }
            float var_18 = var_0[var_11];
            float var_19 = var_1[var_11];
            float var_20 = (var_19 - var_18);
            float var_21 = (var_20 * var_20);
            float var_22 = (var_12 + var_21);
            var_8[0] = var_13;
            var_9[0] = var_22;
        }
        long long int var_23 = var_8[0];
        float var_24 = var_9[0];
        float var_25 = cub::BlockReduce<float,128,cub::BLOCK_REDUCE_WARP_REDUCTIONS,1,1>().Sum(var_24);
        long long int var_26 = threadIdx.x;
        char var_27 = (var_26 == 0);
        if (var_27) {
            long long int var_28 = blockIdx.x;
            char var_29 = (var_28 >= 0);
            char var_31;
            if (var_29) {
                var_31 = (var_28 < 1);
            } else {
                var_31 = 0;
            }
            char var_32 = (var_31 == 0);
            if (var_32) {
                // "Argument out of bounds."
            } else {
            }
            var_2[var_28] = var_25;
        } else {
        }
    }
    __global__ void method_14(float var_0, float var_1, float * var_2, float * var_3, float * var_4) {
        long long int var_5 = blockIdx.x;
        long long int var_6 = (var_5 * 128);
        long long int var_7 = threadIdx.x;
        long long int var_8 = (var_6 + var_7);
        long long int var_9[1];
        var_9[0] = var_8;
        while (method_10(var_9)) {
            long long int var_11 = var_9[0];
            long long int var_12 = (var_11 + 128);
            char var_13 = (var_11 >= 0);
            char var_15;
            if (var_13) {
                var_15 = (var_11 < 8);
            } else {
                var_15 = 0;
            }
            char var_16 = (var_15 == 0);
            if (var_16) {
                // "Argument out of bounds."
            } else {
            }
            char var_18;
            if (var_13) {
                var_18 = (var_11 < 8);
            } else {
                var_18 = 0;
            }
            char var_19 = (var_18 == 0);
            if (var_19) {
                // "Argument out of bounds."
            } else {
            }
            float var_20 = var_2[var_11];
            float var_21 = var_3[var_11];
            float var_22 = var_4[var_11];
            float var_23 = (var_20 - var_21);
            float var_24 = (2 * var_23);
            float var_25 = (var_0 * var_24);
            float var_26 = (var_22 + var_25);
            var_4[var_11] = var_26;
            var_9[0] = var_12;
        }
        long long int var_27 = var_9[0];
    }
    __global__ void method_15(float * var_0, float * var_1, float * var_2, float * var_3) {
        long long int var_4 = blockIdx.x;
        long long int var_5 = (var_4 * 128);
        long long int var_6 = threadIdx.x;
        long long int var_7 = (var_5 + var_6);
        long long int var_8[1];
        var_8[0] = var_7;
        while (method_10(var_8)) {
            long long int var_10 = var_8[0];
            long long int var_11 = (var_10 + 128);
            char var_12 = (var_10 >= 0);
            char var_14;
            if (var_12) {
                var_14 = (var_10 < 8);
            } else {
                var_14 = 0;
            }
            char var_15 = (var_14 == 0);
            if (var_15) {
                // "Argument out of bounds."
            } else {
            }
            char var_17;
            if (var_12) {
                var_17 = (var_10 < 8);
            } else {
                var_17 = 0;
            }
            char var_18 = (var_17 == 0);
            if (var_18) {
                // "Argument out of bounds."
            } else {
            }
            float var_19 = var_0[var_10];
            float var_20 = var_1[var_10];
            float var_21 = var_2[var_10];
            float var_22 = var_3[var_10];
            float var_23 = (1 - var_21);
            float var_24 = (var_21 * var_23);
            float var_25 = (var_20 * var_24);
            float var_26 = (var_22 + var_25);
            var_3[var_10] = var_26;
            var_8[0] = var_11;
        }
        long long int var_27 = var_8[0];
    }
    __global__ void method_17(float * var_0, float * var_1) {
        long long int var_2 = threadIdx.x;
        long long int var_3 = blockIdx.x;
        long long int var_4 = (4 * var_3);
        long long int var_5 = (var_2 + var_4);
        long long int var_6[1];
        var_6[0] = var_5;
        while (method_7(var_6)) {
            long long int var_8 = var_6[0];
            long long int var_9 = (var_8 + 4);
            char var_10 = (var_8 >= 0);
            char var_12;
            if (var_10) {
                var_12 = (var_8 < 4);
            } else {
                var_12 = 0;
            }
            char var_13 = (var_12 == 0);
            if (var_13) {
                // "Argument out of bounds."
            } else {
            }
            char var_15;
            if (var_10) {
                var_15 = (var_8 < 4);
            } else {
                var_15 = 0;
            }
            char var_16 = (var_15 == 0);
            if (var_16) {
                // "Argument out of bounds."
            } else {
            }
            long long int var_17 = threadIdx.y;
            long long int var_18 = blockIdx.y;
            long long int var_19 = (2 * var_18);
            long long int var_20 = (var_17 + var_19);
            float var_21 = 0;
            long long int var_22[1];
            float var_23[1];
            var_22[0] = var_20;
            var_23[0] = var_21;
            while (method_18(var_22, var_23)) {
                long long int var_25 = var_22[0];
                float var_26 = var_23[0];
                long long int var_27 = (var_25 + 2);
                char var_28 = (var_25 >= 0);
                char var_30;
                if (var_28) {
                    var_30 = (var_25 < 2);
                } else {
                    var_30 = 0;
                }
                char var_31 = (var_30 == 0);
                if (var_31) {
                    // "Argument out of bounds."
                } else {
                }
                long long int var_32 = (var_25 * 4);
                char var_34;
                if (var_10) {
                    var_34 = (var_8 < 4);
                } else {
                    var_34 = 0;
                }
                char var_35 = (var_34 == 0);
                if (var_35) {
                    // "Argument out of bounds."
                } else {
                }
                long long int var_36 = (var_32 + var_8);
                float var_37 = var_0[var_36];
                float var_38 = (var_26 + var_37);
                var_22[0] = var_27;
                var_23[0] = var_38;
            }
            long long int var_39 = var_22[0];
            float var_40 = var_23[0];
            __shared__ float var_41[33];
            long long int var_42[1];
            float var_43[1];
            var_42[0] = 2;
            var_43[0] = var_40;
            while (method_19(var_42, var_43)) {
                long long int var_45 = var_42[0];
                float var_46 = var_43[0];
                long long int var_47 = (var_45 / 2);
                long long int var_48 = threadIdx.y;
                char var_49 = (var_48 < var_45);
                char var_52;
                if (var_49) {
                    long long int var_50 = threadIdx.y;
                    var_52 = (var_50 >= var_47);
                } else {
                    var_52 = 0;
                }
                if (var_52) {
                    long long int var_53 = threadIdx.y;
                    char var_54 = (var_53 >= 1);
                    char var_56;
                    if (var_54) {
                        var_56 = (var_53 < 2);
                    } else {
                        var_56 = 0;
                    }
                    char var_57 = (var_56 == 0);
                    if (var_57) {
                        // "Argument out of bounds."
                    } else {
                    }
                    long long int var_58 = (var_53 - 1);
                    long long int var_59 = (var_58 * 33);
                    long long int var_60 = threadIdx.x;
                    char var_61 = (var_60 >= 0);
                    char var_63;
                    if (var_61) {
                        var_63 = (var_60 < 33);
                    } else {
                        var_63 = 0;
                    }
                    char var_64 = (var_63 == 0);
                    if (var_64) {
                        // "Argument out of bounds."
                    } else {
                    }
                    long long int var_65 = (var_59 + var_60);
                    var_41[var_65] = var_46;
                } else {
                }
                __syncthreads();
                long long int var_66 = threadIdx.y;
                char var_67 = (var_66 < var_47);
                float var_92;
                if (var_67) {
                    long long int var_68 = threadIdx.y;
                    long long int var_69 = (var_68 + var_47);
                    long long int var_70[1];
                    float var_71[1];
                    var_70[0] = var_69;
                    var_71[0] = var_46;
                    while (method_20(var_45, var_70, var_71)) {
                        long long int var_73 = var_70[0];
                        float var_74 = var_71[0];
                        long long int var_75 = (var_73 + var_47);
                        char var_76 = (var_73 >= 1);
                        char var_78;
                        if (var_76) {
                            var_78 = (var_73 < 2);
                        } else {
                            var_78 = 0;
                        }
                        char var_79 = (var_78 == 0);
                        if (var_79) {
                            // "Argument out of bounds."
                        } else {
                        }
                        long long int var_80 = (var_73 - 1);
                        long long int var_81 = (var_80 * 33);
                        long long int var_82 = threadIdx.x;
                        char var_83 = (var_82 >= 0);
                        char var_85;
                        if (var_83) {
                            var_85 = (var_82 < 33);
                        } else {
                            var_85 = 0;
                        }
                        char var_86 = (var_85 == 0);
                        if (var_86) {
                            // "Argument out of bounds."
                        } else {
                        }
                        long long int var_87 = (var_81 + var_82);
                        float var_88 = var_41[var_87];
                        float var_89 = (var_74 + var_88);
                        var_70[0] = var_75;
                        var_71[0] = var_89;
                    }
                    long long int var_90 = var_70[0];
                    var_92 = var_71[0];
                } else {
                    var_92 = var_46;
                }
                var_42[0] = var_47;
                var_43[0] = var_92;
            }
            long long int var_93 = var_42[0];
            float var_94 = var_43[0];
            long long int var_95 = threadIdx.y;
            char var_96 = (var_95 == 0);
            if (var_96) {
                float var_97 = var_1[var_8];
                float var_98 = (var_94 + var_97);
                var_1[var_8] = var_98;
            } else {
            }
            var_6[0] = var_9;
        }
        long long int var_99 = var_6[0];
    }
    __device__ char method_7(long long int * var_0) {
        long long int var_1 = var_0[0];
        return (var_1 < 4);
    }
    __device__ char method_8(long long int * var_0) {
        long long int var_1 = var_0[0];
        return (var_1 < 2);
    }
    __device__ char method_10(long long int * var_0) {
        long long int var_1 = var_0[0];
        return (var_1 < 8);
    }
    __device__ char method_12(long long int * var_0, float * var_1) {
        long long int var_2 = var_0[0];
        float var_3 = var_1[0];
        return (var_2 < 8);
    }
    __device__ char method_18(long long int * var_0, float * var_1) {
        long long int var_2 = var_0[0];
        float var_3 = var_1[0];
        return (var_2 < 2);
    }
    __device__ char method_19(long long int * var_0, float * var_1) {
        long long int var_2 = var_0[0];
        float var_3 = var_1[0];
        return (var_2 >= 2);
    }
    __device__ char method_20(long long int var_0, long long int * var_1, float * var_2) {
        long long int var_3 = var_1[0];
        float var_4 = var_2[0];
        return (var_3 < var_0);
    }
}
"""

type Union0 =
    | Union0Case0 of Tuple1
    | Union0Case1
and Tuple1 =
    struct
    val mem_0: ManagedCuda.BasicTypes.CUdeviceptr
    new(arg_mem_0) = {mem_0 = arg_mem_0}
    end
and EnvStack2 =
    struct
    val mem_0: (Union0 ref)
    new(arg_mem_0) = {mem_0 = arg_mem_0}
    end
and Env3 =
    struct
    val mem_0: EnvStack2
    val mem_1: int64
    new(arg_mem_0, arg_mem_1) = {mem_0 = arg_mem_0; mem_1 = arg_mem_1}
    end
let rec method_0 ((var_0: System.Diagnostics.DataReceivedEventArgs)): unit =
    let (var_1: string) = var_0.get_Data()
    let (var_2: string) = System.String.Format("{0}",var_1)
    System.Console.WriteLine(var_2)
and method_1((var_0: (Union0 ref))): ManagedCuda.BasicTypes.CUdeviceptr =
    let (var_1: Union0) = (!var_0)
    match var_1 with
    | Union0Case0(var_2) ->
        var_2.mem_0
    | Union0Case1 ->
        (failwith "A Cuda memory cell that has been disposed has been tried to be accessed.")
and method_2((var_0: uint64), (var_1: System.Collections.Generic.Stack<Env3>), (var_2: uint64), (var_3: int64)): EnvStack2 =
    let (var_4: int32) = var_1.get_Count()
    let (var_5: bool) = (var_4 > 0)
    if var_5 then
        let (var_6: Env3) = var_1.Peek()
        let (var_7: EnvStack2) = var_6.mem_0
        let (var_8: int64) = var_6.mem_1
        let (var_9: (Union0 ref)) = var_7.mem_0
        let (var_10: Union0) = (!var_9)
        match var_10 with
        | Union0Case0(var_11) ->
            let (var_12: ManagedCuda.BasicTypes.CUdeviceptr) = var_11.mem_0
            method_3((var_12: ManagedCuda.BasicTypes.CUdeviceptr), (var_0: uint64), (var_2: uint64), (var_3: int64), (var_1: System.Collections.Generic.Stack<Env3>), (var_7: EnvStack2), (var_8: int64))
        | Union0Case1 ->
            let (var_14: Env3) = var_1.Pop()
            let (var_15: EnvStack2) = var_14.mem_0
            let (var_16: int64) = var_14.mem_1
            method_2((var_0: uint64), (var_1: System.Collections.Generic.Stack<Env3>), (var_2: uint64), (var_3: int64))
    else
        method_4((var_0: uint64), (var_2: uint64), (var_3: int64), (var_1: System.Collections.Generic.Stack<Env3>))
and method_5((var_0: ManagedCuda.CudaBlas.CudaBlasHandle), (var_1: EnvStack2), (var_2: EnvStack2), (var_3: EnvStack2)): unit =
    let (var_4: ManagedCuda.CudaBlas.Operation) = ManagedCuda.CudaBlas.Operation.NonTranspose
    let (var_5: ManagedCuda.CudaBlas.Operation) = ManagedCuda.CudaBlas.Operation.NonTranspose
    let (var_6: (float32 ref)) = (ref 1.000000f)
    let (var_7: (Union0 ref)) = var_1.mem_0
    let (var_8: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_7: (Union0 ref)))
    let (var_9: (Union0 ref)) = var_2.mem_0
    let (var_10: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_9: (Union0 ref)))
    let (var_11: (float32 ref)) = (ref 0.000000f)
    let (var_12: (Union0 ref)) = var_3.mem_0
    let (var_13: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_12: (Union0 ref)))
    let (var_14: ManagedCuda.CudaBlas.CublasStatus) = ManagedCuda.CudaBlas.CudaBlasNativeMethods.cublasSgemm_v2(var_0, var_4, var_5, 4, 2, 6, var_6, var_8, 4, var_10, 6, var_11, var_13, 4)
    if var_14 <> ManagedCuda.CudaBlas.CublasStatus.Success then raise <| new ManagedCuda.CudaBlas.CudaBlasException(var_14)
and method_13((var_0: (float32 [])), (var_1: float32), (var_2: int64)): float32 =
    let (var_3: bool) = (var_2 < 1L)
    if var_3 then
        let (var_4: bool) = (var_2 >= 0L)
        let (var_5: bool) = (var_4 = false)
        if var_5 then
            (failwith "Argument out of bounds.")
        else
            ()
        let (var_6: float32) = var_0.[int32 var_2]
        let (var_7: float32) = (var_1 + var_6)
        let (var_8: int64) = (var_2 + 1L)
        method_13((var_0: (float32 [])), (var_7: float32), (var_8: int64))
    else
        var_1
and method_16((var_0: ManagedCuda.CudaBlas.CudaBlasHandle), (var_1: EnvStack2), (var_2: EnvStack2), (var_3: EnvStack2)): unit =
    let (var_4: ManagedCuda.CudaBlas.Operation) = ManagedCuda.CudaBlas.Operation.NonTranspose
    let (var_5: ManagedCuda.CudaBlas.Operation) = ManagedCuda.CudaBlas.Operation.Transpose
    let (var_6: (float32 ref)) = (ref 1.000000f)
    let (var_7: (Union0 ref)) = var_1.mem_0
    let (var_8: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_7: (Union0 ref)))
    let (var_9: (Union0 ref)) = var_2.mem_0
    let (var_10: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_9: (Union0 ref)))
    let (var_11: (float32 ref)) = (ref 1.000000f)
    let (var_12: (Union0 ref)) = var_3.mem_0
    let (var_13: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_12: (Union0 ref)))
    let (var_14: ManagedCuda.CudaBlas.CublasStatus) = ManagedCuda.CudaBlas.CudaBlasNativeMethods.cublasSgemm_v2(var_0, var_4, var_5, 4, 6, 2, var_6, var_8, 4, var_10, 6, var_11, var_13, 4)
    if var_14 <> ManagedCuda.CudaBlas.CublasStatus.Success then raise <| new ManagedCuda.CudaBlas.CudaBlasException(var_14)
and method_3((var_0: ManagedCuda.BasicTypes.CUdeviceptr), (var_1: uint64), (var_2: uint64), (var_3: int64), (var_4: System.Collections.Generic.Stack<Env3>), (var_5: EnvStack2), (var_6: int64)): EnvStack2 =
    let (var_7: ManagedCuda.BasicTypes.SizeT) = var_0.Pointer
    let (var_8: uint64) = uint64 var_7
    let (var_9: uint64) = uint64 var_6
    let (var_10: int64) = (var_3 % 256L)
    let (var_11: int64) = (var_3 - var_10)
    let (var_12: int64) = (var_11 + 256L)
    let (var_13: uint64) = (var_8 + var_9)
    let (var_14: uint64) = (var_1 + var_2)
    let (var_15: uint64) = uint64 var_12
    let (var_16: uint64) = (var_14 - var_13)
    let (var_17: bool) = (var_15 <= var_16)
    let (var_18: bool) = (var_17 = false)
    if var_18 then
        (failwith "Cache size has been exceeded in the allocator.")
    else
        ()
    let (var_19: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(var_13)
    let (var_20: ManagedCuda.BasicTypes.CUdeviceptr) = ManagedCuda.BasicTypes.CUdeviceptr(var_19)
    let (var_21: (Union0 ref)) = (ref (Union0Case0(Tuple1(var_20))))
    let (var_22: EnvStack2) = EnvStack2((var_21: (Union0 ref)))
    var_4.Push((Env3(var_22, var_12)))
    var_22
and method_4((var_0: uint64), (var_1: uint64), (var_2: int64), (var_3: System.Collections.Generic.Stack<Env3>)): EnvStack2 =
    let (var_4: int64) = (var_2 % 256L)
    let (var_5: int64) = (var_2 - var_4)
    let (var_6: int64) = (var_5 + 256L)
    let (var_7: uint64) = (var_0 + var_1)
    let (var_8: uint64) = uint64 var_6
    let (var_9: uint64) = (var_7 - var_0)
    let (var_10: bool) = (var_8 <= var_9)
    let (var_11: bool) = (var_10 = false)
    if var_11 then
        (failwith "Cache size has been exceeded in the allocator.")
    else
        ()
    let (var_12: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(var_0)
    let (var_13: ManagedCuda.BasicTypes.CUdeviceptr) = ManagedCuda.BasicTypes.CUdeviceptr(var_12)
    let (var_14: (Union0 ref)) = (ref (Union0Case0(Tuple1(var_13))))
    let (var_15: EnvStack2) = EnvStack2((var_14: (Union0 ref)))
    var_3.Push((Env3(var_15, var_6)))
    var_15
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
let (var_8: string) = System.IO.Path.Combine("C:/Program Files (x86)/Microsoft Visual Studio/2017/Community", "VC/Auxiliary/Build/vcvars64.bat")
let (var_9: string) = System.IO.Path.Combine("C:/Program Files (x86)/Microsoft Visual Studio/2017/Community", "VC/Tools/MSVC/14.11.25503/bin/Hostx64/x64")
let (var_10: string) = System.IO.Path.Combine("C:/Program Files/NVIDIA GPU Computing Toolkit/CUDA/v9.0", "include")
let (var_11: string) = System.IO.Path.Combine("C:/Program Files (x86)/Microsoft Visual Studio/2017/Community", "VC/Tools/MSVC/14.11.25503/include")
let (var_12: string) = System.IO.Path.Combine("C:/Program Files/NVIDIA GPU Computing Toolkit/CUDA/v9.0", "bin/nvcc.exe")
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
let (var_21: string) = String.concat "" [|"\""; var_12; "\" -gencode=arch=compute_52,code=\\\"sm_52,compute_52\\\" --use-local-env --cl-version 2017 -I\""; var_10; "\" -I\"C:/cub-1.7.4\" -I\""; var_11; "\" --keep-dir \""; var_2; "\" -maxrregcount=0  --machine 64 -ptx -cudart static  -o \""; var_13; "\" \""; var_14; "\""|]
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
    let (var_29: string) = String.concat ", " [|"NVCC failed compilation."; var_28|]
    let (var_30: string) = System.String.Format("[{0}]",var_29)
    (failwith var_30)
else
    ()
let (var_31: System.TimeSpan) = var_22.get_Elapsed()
printfn "The time it took to compile the Cuda kernels is: %A" var_31
let (var_32: ManagedCuda.BasicTypes.CUmodule) = var_1.LoadModulePTX(var_13)
var_5.Dispose()
let (var_33: string) = String.concat "" [|"Compiled the kernels into the following directory: "; var_2|]
let (var_34: string) = System.String.Format("{0}",var_33)
System.Console.WriteLine(var_34)
let (var_35: ManagedCuda.CudaDeviceProperties) = var_1.GetDeviceInfo()
let (var_36: ManagedCuda.BasicTypes.SizeT) = var_35.get_TotalGlobalMemory()
let (var_37: int64) = int64 var_36
let (var_38: float) = float var_37
let (var_39: float) = (0.700000 * var_38)
let (var_40: int64) = int64 var_39
let (var_41: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(var_40)
let (var_42: ManagedCuda.BasicTypes.CUdeviceptr) = var_1.AllocateMemory(var_41)
let (var_43: (Union0 ref)) = (ref (Union0Case0(Tuple1(var_42))))
let (var_44: EnvStack2) = EnvStack2((var_43: (Union0 ref)))
let (var_45: System.Collections.Generic.Stack<Env3>) = System.Collections.Generic.Stack<Env3>()
let (var_46: (Union0 ref)) = var_44.mem_0
let (var_47: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_46: (Union0 ref)))
let (var_48: ManagedCuda.BasicTypes.SizeT) = var_47.Pointer
let (var_49: uint64) = uint64 var_48
let (var_50: uint64) = uint64 var_40
let (var_51: ManagedCuda.CudaStream) = ManagedCuda.CudaStream()
let (var_52: ManagedCuda.CudaRand.GeneratorType) = ManagedCuda.CudaRand.GeneratorType.PseudoDefault
let (var_53: ManagedCuda.CudaRand.CudaRandDevice) = ManagedCuda.CudaRand.CudaRandDevice(var_52)
let (var_54: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
var_53.SetStream(var_54)
let (var_55: ManagedCuda.CudaBlas.PointerMode) = ManagedCuda.CudaBlas.PointerMode.Host
let (var_56: ManagedCuda.CudaBlas.AtomicsMode) = ManagedCuda.CudaBlas.AtomicsMode.Allowed
let (var_57: ManagedCuda.CudaBlas.CudaBlas) = ManagedCuda.CudaBlas.CudaBlas(var_55, var_56)
let (var_58: ManagedCuda.CudaBlas.CudaBlasHandle) = var_57.get_CublasHandle()
let (var_59: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
var_57.set_Stream(var_59)
let (var_60: int64) = 48L
let (var_61: EnvStack2) = method_2((var_49: uint64), (var_45: System.Collections.Generic.Stack<Env3>), (var_50: uint64), (var_60: int64))
let (var_62: (Union0 ref)) = var_61.mem_0
let (var_63: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_62: (Union0 ref)))
let (var_64: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(12L)
var_53.GenerateNormal32(var_63, var_64, 0.000000f, 1.000000f)
let (var_65: int64) = 32L
let (var_66: EnvStack2) = method_2((var_49: uint64), (var_45: System.Collections.Generic.Stack<Env3>), (var_50: uint64), (var_65: int64))
let (var_67: (Union0 ref)) = var_66.mem_0
let (var_68: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_67: (Union0 ref)))
let (var_69: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
let (var_70: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(32L)
var_1.ClearMemoryAsync(var_68, 0uy, var_70, var_69)
let (var_71: int64) = 96L
let (var_72: EnvStack2) = method_2((var_49: uint64), (var_45: System.Collections.Generic.Stack<Env3>), (var_50: uint64), (var_71: int64))
let (var_73: (Union0 ref)) = var_72.mem_0
let (var_74: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_73: (Union0 ref)))
let (var_75: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(24L)
var_53.GenerateNormal32(var_74, var_75, 0.000000f, 0.447214f)
let (var_76: int64) = 96L
let (var_77: EnvStack2) = method_2((var_49: uint64), (var_45: System.Collections.Generic.Stack<Env3>), (var_50: uint64), (var_76: int64))
let (var_78: (Union0 ref)) = var_77.mem_0
let (var_79: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_78: (Union0 ref)))
let (var_80: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
let (var_81: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(96L)
var_1.ClearMemoryAsync(var_79, 0uy, var_81, var_80)
let (var_82: int64) = 16L
let (var_83: EnvStack2) = method_2((var_49: uint64), (var_45: System.Collections.Generic.Stack<Env3>), (var_50: uint64), (var_82: int64))
let (var_84: (Union0 ref)) = var_83.mem_0
let (var_85: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_84: (Union0 ref)))
let (var_86: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
let (var_87: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(16L)
var_1.ClearMemoryAsync(var_85, 0uy, var_87, var_86)
let (var_88: int64) = 16L
let (var_89: EnvStack2) = method_2((var_49: uint64), (var_45: System.Collections.Generic.Stack<Env3>), (var_50: uint64), (var_88: int64))
let (var_90: (Union0 ref)) = var_89.mem_0
let (var_91: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_90: (Union0 ref)))
let (var_92: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
let (var_93: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(16L)
var_1.ClearMemoryAsync(var_91, 0uy, var_93, var_92)
let (var_94: int64) = 32L
let (var_95: EnvStack2) = method_2((var_49: uint64), (var_45: System.Collections.Generic.Stack<Env3>), (var_50: uint64), (var_94: int64))
method_5((var_58: ManagedCuda.CudaBlas.CudaBlasHandle), (var_72: EnvStack2), (var_61: EnvStack2), (var_95: EnvStack2))
let (var_96: int64) = 32L
let (var_97: EnvStack2) = method_2((var_49: uint64), (var_45: System.Collections.Generic.Stack<Env3>), (var_50: uint64), (var_96: int64))
let (var_98: (Union0 ref)) = var_97.mem_0
let (var_99: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_98: (Union0 ref)))
let (var_100: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
let (var_101: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(32L)
var_1.ClearMemoryAsync(var_99, 0uy, var_101, var_100)
let (var_102: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_84: (Union0 ref)))
let (var_103: (Union0 ref)) = var_95.mem_0
let (var_104: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_103: (Union0 ref)))
let (var_105: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_103: (Union0 ref)))
// Cuda join point
// method_6((var_102: ManagedCuda.BasicTypes.CUdeviceptr), (var_104: ManagedCuda.BasicTypes.CUdeviceptr), (var_105: ManagedCuda.BasicTypes.CUdeviceptr))
let (var_106: ManagedCuda.CudaKernel) = ManagedCuda.CudaKernel("method_6", var_32, var_1)
let (var_107: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(1u, 1u, 1u)
var_106.set_GridDimensions(var_107)
let (var_108: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(4u, 2u, 1u)
var_106.set_BlockDimensions(var_108)
let (var_109: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
let (var_111: (System.Object [])) = [|var_102; var_104; var_105|]: (System.Object [])
var_106.RunAsync(var_109, var_111)
let (var_116: int64) = 32L
let (var_117: EnvStack2) = method_2((var_49: uint64), (var_45: System.Collections.Generic.Stack<Env3>), (var_50: uint64), (var_116: int64))
let (var_118: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_103: (Union0 ref)))
let (var_119: (Union0 ref)) = var_117.mem_0
let (var_120: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_119: (Union0 ref)))
// Cuda join point
// method_9((var_118: ManagedCuda.BasicTypes.CUdeviceptr), (var_120: ManagedCuda.BasicTypes.CUdeviceptr))
let (var_121: ManagedCuda.CudaKernel) = ManagedCuda.CudaKernel("method_9", var_32, var_1)
let (var_122: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(1u, 1u, 1u)
var_121.set_GridDimensions(var_122)
let (var_123: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(128u, 1u, 1u)
var_121.set_BlockDimensions(var_123)
let (var_124: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
let (var_126: (System.Object [])) = [|var_118; var_120|]: (System.Object [])
var_121.RunAsync(var_124, var_126)
let (var_127: int64) = 32L
let (var_128: EnvStack2) = method_2((var_49: uint64), (var_45: System.Collections.Generic.Stack<Env3>), (var_50: uint64), (var_127: int64))
let (var_129: (Union0 ref)) = var_128.mem_0
let (var_130: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_129: (Union0 ref)))
let (var_131: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
let (var_132: ManagedCuda.BasicTypes.SizeT) = ManagedCuda.BasicTypes.SizeT(32L)
var_1.ClearMemoryAsync(var_130, 0uy, var_132, var_131)
let (var_133: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_119: (Union0 ref)))
let (var_134: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_67: (Union0 ref)))
let (var_137: int64) = 4L
let (var_138: EnvStack2) = method_2((var_49: uint64), (var_45: System.Collections.Generic.Stack<Env3>), (var_50: uint64), (var_137: int64))
let (var_139: (Union0 ref)) = var_138.mem_0
let (var_140: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_139: (Union0 ref)))
// Cuda join point
// method_11((var_133: ManagedCuda.BasicTypes.CUdeviceptr), (var_134: ManagedCuda.BasicTypes.CUdeviceptr), (var_140: ManagedCuda.BasicTypes.CUdeviceptr))
let (var_141: ManagedCuda.CudaKernel) = ManagedCuda.CudaKernel("method_11", var_32, var_1)
let (var_142: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(1u, 1u, 1u)
var_141.set_GridDimensions(var_142)
let (var_143: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(128u, 1u, 1u)
var_141.set_BlockDimensions(var_143)
let (var_144: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
let (var_146: (System.Object [])) = [|var_133; var_134; var_140|]: (System.Object [])
var_141.RunAsync(var_144, var_146)
let (var_147: (float32 [])) = Array.zeroCreate<float32> (System.Convert.ToInt32(1L))
let (var_148: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_139: (Union0 ref)))
var_1.CopyToHost(var_147, var_148)
let (var_149: float32) = 0.000000f
let (var_150: int64) = 0L
let (var_151: float32) = method_13((var_147: (float32 [])), (var_149: float32), (var_150: int64))
var_139 := Union0Case1
let (var_152: (float32 ref)) = (ref 0.000000f)
let (var_153: float32) = (var_151 / 2.000000f)
let (var_154: (float32 ref)) = (ref 0.000000f)
let (var_155: string) = System.String.Format("{0}",var_153)
let (var_156: string) = String.concat ", " [|"Cost is:"; var_155|]
let (var_157: string) = System.String.Format("[{0}]",var_156)
System.Console.WriteLine(var_157)
var_154 := 1.000000f
let (var_158: float32) = (!var_154)
let (var_159: float32) = (var_158 / 2.000000f)
let (var_160: float32) = (!var_152)
let (var_161: float32) = (var_160 + var_159)
var_152 := var_161
let (var_162: float32) = (!var_152)
let (var_163: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_119: (Union0 ref)))
let (var_164: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_67: (Union0 ref)))
let (var_165: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_129: (Union0 ref)))
// Cuda join point
// method_14((var_162: float32), (var_151: float32), (var_163: ManagedCuda.BasicTypes.CUdeviceptr), (var_164: ManagedCuda.BasicTypes.CUdeviceptr), (var_165: ManagedCuda.BasicTypes.CUdeviceptr))
let (var_166: ManagedCuda.CudaKernel) = ManagedCuda.CudaKernel("method_14", var_32, var_1)
let (var_167: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(1u, 1u, 1u)
var_166.set_GridDimensions(var_167)
let (var_168: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(128u, 1u, 1u)
var_166.set_BlockDimensions(var_168)
let (var_169: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
let (var_171: (System.Object [])) = [|var_162; var_151; var_163; var_164; var_165|]: (System.Object [])
var_166.RunAsync(var_169, var_171)
let (var_172: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_103: (Union0 ref)))
let (var_173: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_129: (Union0 ref)))
let (var_174: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_119: (Union0 ref)))
let (var_175: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_98: (Union0 ref)))
// Cuda join point
// method_15((var_172: ManagedCuda.BasicTypes.CUdeviceptr), (var_173: ManagedCuda.BasicTypes.CUdeviceptr), (var_174: ManagedCuda.BasicTypes.CUdeviceptr), (var_175: ManagedCuda.BasicTypes.CUdeviceptr))
let (var_176: ManagedCuda.CudaKernel) = ManagedCuda.CudaKernel("method_15", var_32, var_1)
let (var_177: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(1u, 1u, 1u)
var_176.set_GridDimensions(var_177)
let (var_178: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(128u, 1u, 1u)
var_176.set_BlockDimensions(var_178)
let (var_179: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
let (var_181: (System.Object [])) = [|var_172; var_173; var_174; var_175|]: (System.Object [])
var_176.RunAsync(var_179, var_181)
method_16((var_58: ManagedCuda.CudaBlas.CudaBlasHandle), (var_97: EnvStack2), (var_61: EnvStack2), (var_77: EnvStack2))
let (var_182: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_98: (Union0 ref)))
let (var_183: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_90: (Union0 ref)))
// Cuda join point
// method_17((var_182: ManagedCuda.BasicTypes.CUdeviceptr), (var_183: ManagedCuda.BasicTypes.CUdeviceptr))
let (var_184: ManagedCuda.CudaKernel) = ManagedCuda.CudaKernel("method_17", var_32, var_1)
let (var_185: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(1u, 1u, 1u)
var_184.set_GridDimensions(var_185)
let (var_186: ManagedCuda.VectorTypes.dim3) = ManagedCuda.VectorTypes.dim3(4u, 2u, 1u)
var_184.set_BlockDimensions(var_186)
let (var_187: ManagedCuda.BasicTypes.CUstream) = var_51.get_Stream()
let (var_189: (System.Object [])) = [|var_182; var_183|]: (System.Object [])
var_184.RunAsync(var_187, var_189)
var_129 := Union0Case1
var_119 := Union0Case1
var_98 := Union0Case1
var_103 := Union0Case1
var_90 := Union0Case1
var_84 := Union0Case1
var_78 := Union0Case1
var_73 := Union0Case1
var_67 := Union0Case1
var_62 := Union0Case1
var_57.Dispose()
var_53.Dispose()
var_51.Dispose()
let (var_190: ManagedCuda.BasicTypes.CUdeviceptr) = method_1((var_46: (Union0 ref)))
var_1.FreeMemory(var_190)
var_46 := Union0Case1
var_1.Dispose()

