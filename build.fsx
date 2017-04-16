// include Fake lib
#r "packages/build/FAKE/tools/FakeLib.dll"
open Fake

// Properties
let buildDir = FullName "./build/"

let apikey = getBuildParam "apikey"
let version = getBuildParam "version"

Target "Clean" (fun _ ->
   CleanDir buildDir
)

Target "Build" (fun _ -> 
   XMLHelper.XmlPokeInnerText "./BarsGroup.CodeGuard/BarsGroup.CodeGuard.csproj" "/Project/PropertyGroup/Version" version

   DotNetCli.Restore (fun p -> p)

   DotNetCli.Build (fun p -> 
   { p with
      Configuration = "Release"
   })

   ()

   DotNetCli.Pack (fun p -> 
   { p with
      OutputPath = buildDir
      Project = "./BarsGroup.CodeGuard/BarsGroup.CodeGuard.csproj"
   })
)

Target "RunTests" (fun _ -> 
    DotNetCli.Test (fun p -> 
   { p with
      Project = "./BarsGroup.CodeGuard.Tests/BarsGroup.CodeGuard.Tests.csproj"
   })
    
)

Target "PublishNuget" (fun _ -> 
   let package = !! "./build/BarsGroup.CodeGuard.*.nupkg"
   
   Paket.Push (fun nugetParams -> 
    { nugetParams with
        ApiKey = apikey
        WorkingDir = buildDir
    }
   )
)

Target "Default" (fun _ ->
   trace "Hello World from FAKE"
)

// Dependencies
"Clean"
   ==> "Build"
   ==> "RunTests"
   ==> "PublishNuget"
   ==> "Default"

// start build
RunTargetOrDefault "Default"