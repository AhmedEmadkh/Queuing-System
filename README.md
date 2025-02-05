# Queue System Simulation

This project simulates various queueing systems, including M/M/1, M/M/1/K, M/M/C, and M/M/C/K, to model and analyze their behavior under different conditions. It is designed to help users understand the dynamics of queueing systems and their performance metrics, such as average waiting time, queue length, and system utilization.

## Supported Queueing Models
- **M/M/1**: Single-server queue with infinite capacity.
- **M/M/1/K**: Single-server queue with finite capacity `K`.
- **M/M/C**: Multi-server queue with infinite capacity and `C` servers.
- **M/M/C/K**: Multi-server queue with finite capacity `K` and `C` servers.

## Features
- Simulate queueing systems with customizable parameters:
  - Arrival rate (λ)
  - Service rate (μ)
  - Number of servers (C)
  - Queue capacity (K)
- Calculate performance metrics:
  - Average queue length
  - Average waiting time
  - System utilization
  - Probability of blocking (for finite-capacity systems)
- Extensible framework for adding new queueing models.

## Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/AhmedEmadkh/Queuing-System.git
