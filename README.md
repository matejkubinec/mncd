![MNCD](./img/MNCD.png)

# MNCD | Multi-layer Network Community Detection

MNCD is a .NET library written in C#, used for community detection
in multi-layered networks.

## Installation

MNCD is available through [nuget](https://www.nuget.org/packages/mncd).

## Examples

- [Network Creation](./examples/network.ipynb)
- [Flattening](./examples/flattening.ipynb)
- [Single Layer](./examples/single-layer.ipynb)
- [Multi Layer](./examples/multi-layer.ipynb)
- [Evaluation](./examples/evaluation.ipynb)

## Overview

Implemented are several approaches to apply community detection on a network.

## Single-Layer

There are two main methods to apply single-layer community detection algorithms
on multi-layered network.

MNCD implements these single-layer community detection algorithms:

- [Louvain](./src/MNCD/CommunityDetection/SingleLayer/Louvain.cs)
- [FluidC](./src/MNCD/CommunityDetection/SingleLayer/FluidC.cs)
- [KClique](./src/MNCD/CommunityDetection/SingleLayer/KClique.cs)
- [Label Propagation](./src/MNCD/CommunityDetection/SingleLayer/LabelPropagation.cs)

### One Layer Only

Community detection is applied only on one selected layer.

### Flattening

Network is flattened and then a single-layer community detection algorithm is
applied.

MNCD implements these flattening methods:

- [Basic Flattening](./src/MNCD/Flattening/BasicFlattening.cs)
- [Merge Flattening](./src/MNCD/Flattening/MergeFlattening.cs)
- [Weighted Flattening](./src/MNCD/Flattening/WeightedFlattening.cs)
- [Local Simplification](./src/MNCD/Flattening/LocalSimplification.cs)

## Multi-Layer

MNCD implements two multi-layer community detection algorithms:

- [ABACUS](./src/MNCD/CommunityDetection/MultiLayer/ABACUS.cs)
- [CLECC Community Detection](./src/MNCD/CommunityDetection/MultiLayer/CLECCCommunityDetection.cs)

## Measures

MNCD implements also measures for evaluating detected communities:

- Single-Layer
  - [Coverage](./src/MNCD/Evaluation/SingleLayer/Coverage.cs)
  - [Performance](./src/MNCD/Evaluation/SingleLayer/Performance.cs)
  - [Modularity](./src/MNCD/Evaluation/SingleLayer/Modularity.cs)
- Multi-Layer
  - [Complementarity](./src/MNCD/Evaluation/MultiLayer/Complementarity.cs)
  - [Exclusivity](./src/MNCD/Evaluation/MultiLayer/Exclusivity.cs)
  - [Homogenity](./src/MNCD/Evaluation/MultiLayer/Homogenity.cs)
  - [Redundancy](./src/MNCD/Evaluation/MultiLayer/Redundancy.cs)
  - [Variety](./src/MNCD/Evaluation/MultiLayer/Variety.cs)
