# Project Overview
---

## Application Hierarchy
![Project Structure](../static/project_diagram.png)

## Project Structure
This application strives to keep application code separated using the principles of
[Clean Architecture](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architectureI)

The TL;DR of this is:
- Separate code by concern. (Ex: database models do not belong in the Networking project)
- Attempt to stick to SOLID principles by making each function have **one** purpose only. Split logic into private singular functions
if necessary
- Project structure should closely resemble an "onion" by separating logic into new projects/libraries as deemed necessary
- Follow the [Rule of Three](https://en.wikipedia.org/wiki/Rule_of_three_(computer_programming)) when creating abstractions
