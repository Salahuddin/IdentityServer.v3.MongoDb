if($env:APPVEYOR_REPO_TAG -eq 'True'){
    nuget pack core.nuspec -version $env:APPVEYOR_REPO_BRANCH
    nuget pack AdminModule.nuspec -version $env:APPVEYOR_REPO_BRANCH
}