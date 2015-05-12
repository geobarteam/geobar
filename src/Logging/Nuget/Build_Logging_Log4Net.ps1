$name = "Logging.Log4Net"
if (Test-Path ".\$name")
{
    del ".\$name" -Recurse
}

mkdir ".\$name"
mkdir ".\$name\lib"
mkdir ".\$name\content"

cp  ".\GoSimple.$name.nuspec" ".\$name"
cp "..\Log4Net\bin\Release\GoSimple.$name.dll" ".\$name\lib\"
cp "..\Sample\Log4Net.config" ".\$name\content\"


nuget pack ".\$name\GoSimple.$name.nuspec"