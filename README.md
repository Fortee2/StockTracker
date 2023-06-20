# StockTracker

A .NET library for calculating common technical indicators for stock analysis.   Library is currently in .NET 6.

## Available Indicators

- Simple Moving Averages 
- Exponetial Moving Averages (EMA)
- Moving Average Convergence Divergence (MACD)
- Realitive Strength Index (RSI)
- Slope (Price Direction)
- True Range
- Directional Movement

## Available Candlestick Patterns
- Hammer Pattern Detection 

## Installation

## Project Structure

L Core  
  L Analyzers - This directory contains Candlestick Patterns
  L Calculators - Classes that calculate the values for a technical indicator
    L Response - Classes that define the return from calculators
  L Domain - Data Structures to use for inputs into calculators
  L Interfaces - Contracts implemented by the Calculators
L CoreTests - NUnit testing suite for the Core components

## Building the Project
