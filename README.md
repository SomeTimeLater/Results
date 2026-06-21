# Results
The package introduces **Results** to represent the outcome of operations, 
allowing for clear handling of success, failure, and error propagation. 

There are two categories of results:

- **Non-typed results:** These contain general information about the operation, including:
    - Success status
    - Failure types (if any)
    - Error messages

- **Typed results:** These inherit all functionality from non-typed results, in addition to:
    - The result's output type
    - The result's output (will throw error if the result is a failure)

## 📦 Installation

```powershell
dotnet add package SomeTimeLater.Results
