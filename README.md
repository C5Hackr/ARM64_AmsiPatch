# ARM64 AMSI Patch

With the rise of ARM64 as an emerging architecture for Windows on ARM devices, there is an increasing need to understand and adapt low-level techniques traditionally used on x86_64 systems to this new platform. This repository demonstrates how AMSI (Antimalware Scan Interface) patching can be translated to ARM64, showcasing the fundamental differences and similarities in opcode manipulation between x86_64 and ARM64 architectures.

## Key Highlights
- **Focus on ARM64**: Showcases the application of AMSI bypass techniques specifically tailored for the ARM64 architecture, which is becoming more common in the Windows ecosystem.
- **Opcode Analysis**: Offers examples and explanations of how patching opcodes on ARM64 differs from x86_64, helping bridge the gap for those familiar with x86 internals.
- **Windows on ARM Research**: Contributes to the growing body of knowledge around system-level research on Windows ARM64 platforms and learn how ARM64 opcode patching works and how it compares to x86_64 patching, offering useful knowledge into architecture specific low-level operations.

## Why ARM64?
Windows on ARM is rapidly gaining traction with devices like the Surface Pro X and other Snapdragon-powered systems. As more software and security solutions are ported to ARM64, understanding the underlying architecture is crucial for both offensive and defensive security research.

## Why AMSI?
AMSI was chosen as the focus for this project because it represents a straightforward and well-known example for demonstrating binary patching techniques. It serves as an accessible starting point to illustrate the transition from x86_64 to ARM64 patching. Additionally, there is currently a noticeable lack of publicly available research or practical examples specifically covering AMSI patching on ARM64, positioning this work as a unique contribution to the community.

## Disclaimer
This repository is intended for educational and research purposes only. Use this knowledge responsibly.
#
#
# AMSI Patch Guide: x86_64 → ARM64

This guide explains how a common AMSI bypass patch is translated from x86_64 to ARM64, focusing on opcode differences and achieving the same functional result.

## Conceptual Overview:

```plaintext
┌───────────────────────────────────────────────────────────┐
│ Goal: Bypass AMSI by making AmsiScanBuffer return E_INVALIDARG (0x80070057) │
└───────────────────────────────────────────────────────────┘
```

### x86_64 Patch:
```asm
mov eax, 0x80070057 ; Set return value in EAX register
ret ; Return from function
```

**Machine Code (Hex):**
```asm
B8 57 00 07 80 C3
```

**Explanation:**
- `mov eax, 0x80070057` → `B8 57 00 07 80`
- `ret` → `C3`

### ARM64 Patch:
ARM64 uses a different instruction set. We need to set the return value in `x0` and return.
```asm
movz x0, #0x0057 ; Load lower 16 bits
movk x0, #0x8007, lsl #16 ; Load upper 16 bits
ret ; Return from function
```

**Machine Code (Hex):**
```asm
40 00 80 D2 ; movz x0, #0x57
00 1C 88 F2 ; movk x0, #0x8007, lsl #16
C0 03 5F D6 ; ret
```

## Visual Breakdown:

```asm
┌───────────────────────────────────────┐
│ x86_64 (Simple)                       │
├───────────────────────────────────────┤
│ mov eax, 0x80070057                   │
│ ret                                   │
│                                       │
│ **Hex: B8 57 00 07 80 C3**            │
└───────────────────────────────────────┘
```

```asm
┌───────────────────────────────────────┐
│ ARM64 (Immediate Encoding Needed)     │
├───────────────────────────────────────┤
│ movz x0, #0x0057                      │
│ movk x0, #0x8007, lsl #16             │
│ ret                                   │
│                                       │
│ **Hex: 40 00 80 D2 00 1C 88 F2 C0 03 5F D6** │
└───────────────────────────────────────┘
```

## Why the Extra Instructions on ARM64?
- **x86_64**: `mov eax, imm32` can load a 32-bit value directly.
- **ARM64**: Immediate values are encoded differently, large values require `movz` + `movk` to construct a 32-bit constant.

## End Result:
Both patches achieve the same outcome:
- **x86_64 → Set `eax` to `0x80070057` → Return**
- **ARM64 → Set `x0` to `0x80070057` → Return**

When patching, these byte sequences can be directly written into the AmsiScanBuffer prologue to bypass scanning.

## Additionally, a conversion table for x86_64 to ARM64 registers can be found below.
```asm
x64 (x86-64)        ->   ARM64 (AArch64)
----------------------------------------
RSP  (Stack Ptr)    ->   SP  (Stack Ptr)
RBP  (Frame Ptr)    ->   FP  (X29 - Frame Ptr)
RIP  (Instr Ptr)    ->   PC  (Program Counter)
RAX  (General Reg)  ->   X0  (General Reg)
RBX  (General Reg)  ->   X1  (General Reg)
RCX  (General Reg)  ->   X2  (General Reg)
RDX  (General Reg)  ->   X3  (General Reg)
RSI  (General Reg)  ->   X4  (General Reg)
RDI  (General Reg)  ->   X5  (General Reg)
R8   (General Reg)  ->   X6  (General Reg)
R9   (General Reg)  ->   X7  (General Reg)
R10  (General Reg)  ->   X8  (General Reg)
R11  (General Reg)  ->   X9  (General Reg)
R12  (General Reg)  ->   X10 (General Reg)
R13  (General Reg)  ->   X11 (General Reg)
R14  (General Reg)  ->   X12 (General Reg)
R15  (General Reg)  ->   X13 (General Reg)
                        X14 (General Reg)
                        X15 (General Reg)
                        X16 (Scratch Reg)
                        X17 (Scratch Reg)
                        X18 (Platform Reg)
                        X19 (Saved Reg)
                        X20 (Saved Reg)
                        X21 (Saved Reg)
                        X22 (Saved Reg)
                        X23 (Saved Reg)
                        X24 (Saved Reg)
                        X25 (Saved Reg)
                        X26 (Saved Reg)
                        X27 (Saved Reg)
                        X28 (Saved Reg)
                        X30 (Link Register)

EFLAGS (Status Reg) ->   CPSR (Current Program Status Reg)

Dr0 - Dr3 (Debug)   ->   Bcr0 - Bcr7 (Breakpoint Registers)
Dr6, Dr7 (Debug)    ->   Bvr0 - Bvr7 (Breakpoint Value Registers)
                        Wcr0 - Wcr1 (Watchpoint Registers)
                        Wvr0 - Wvr1 (Watchpoint Value Registers)

FltSave (FPU/SIMD)  ->   V0 - V31 (128-bit SIMD & Floating Point Registers)
                        FPCR (Floating Point Control Register)
                        FPSR (Floating Point Status Register)
```
