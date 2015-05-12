if (Test-Path .\Logging)
{
    del .\logging -Recurse
}

mkdir .\Logging
mkdir .\Logging\lib

cp  .\GoSimple.Logging.nuspec .\Logging
cp ..\Logging\bin\Release\GoSimple.Logging.dll .\Logging\lib\

nuget pack .\Logging\GoSimple.Logging.nuspec

