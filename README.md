# Beamable Unity Sample Project

### Sample Unity Project using some of Beamable Features and Services

This project includes sample code for the following Beamable services:
- **Currency Sample** - Managing in-game currencies (soft and hard currency)
- **Content Sample as Remote Config** - Using Beamable content as remote configuration
- **Player Stats Sample** - Tracking and managing player statistics
- **Player Auth Sample** - Player authentication implementation

## Prerequisites

- Unity Editor 6000.0.59f2 or compatible version
- .NET Framework 4.7.1
- Beamable SDK

## Getting Started

1. Clone this repository
2. Open the project in Unity
3. Configure your Beamable credentials
4. Open the sample scene in `Assets/Scenes`
5. Run the project

## Project Structure

- `Assets/Scripts/Controllers/` - Contains controller scripts for each Beamable service sample
    - `PlayerCurrencyController.cs` - Currency management implementation
    - `RemoteConfigController.cs` - Remote configuration handling
    - `PlayerStatsController.cs` - Player statistics tracking
    - `PlayerAuthController.cs` - Authentication flow
- `Assets/Scenes/` - Sample scenes demonstrating the features
- `BeamableServices/` - Custom microservices (if applicable)

## Features

Each sample demonstrates both client-side and server-side (microservice) implementations where applicable, showing best practices for integrating Beamable services into your Unity game.

## Resources

- [Beamable Documentation](https://docs.beamable.com/)
- [Beamable Unity SDK](https://github.com/beamable/BeamableProduct)
