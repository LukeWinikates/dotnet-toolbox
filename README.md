# .NET Toolbox

## Goals of this Project
1. As a product, provide a resource for .NET developers who want to find useful libraries for building .NET applications, especially FOSS libraries. When in doubt, do what ruby-toolbox.org does.
1. As a codebase, demonstrate patterns for building .NET applications with maximal developer productivity and happiness
  1. using the cross-platform `dnx` runtime
  1. using the 12-factor style and deploying to a PaaS 
  1. using Nodejs-based tooling for HTML, CSS, and JavaScript to reduce incidental complexity
  1. using a C# editor available on non-Windows platforms (VS Code)


## Getting Started

To build the project, you'll need Node.js and npm. You can install node via Homebrew
 
`brew install node` 
 
Or using [nvm](https://github.com/creationix/nvm)

Once that's done, you'll need to install the gulp command line utility

`npm install -g gulp` 
 
 and the npm dependencies of the ui project
 
 `cd dotnet-toolbox.ui; npm install`. 