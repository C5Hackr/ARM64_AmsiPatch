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
