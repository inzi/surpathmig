set "targetFolder=Y:\Desktop\RDP"
set "tagVersion=Release_"

cd C:\Dev\Surpath\SurPath.Web\bin\Release\Debug
7z a -tzip %tagVersion%web.zip * -x!*.config
move %tagVersion%web.zip "%targetFolder%"

cd C:\Dev\Surpath\Surpath.Backend\SurpathBackend\bin\Debug
7z a -tzip %tagVersion%backend.zip * -x!*.config
move %tagVersion%backend.zip "%targetFolder%"

cd C:\Dev\Surpath\SurPath\bin\Debug
7z a -tzip %tagVersion%surpathwin.zip * -x!*.config
move %tagVersion%surpathwin.zip "%targetFolder%"

cd C:\Dev\Surpath\HL7.Parser\bin\Debug\
7z a -tzip %tagVersion%parser.zip * -x!*.config
move %tagVersion%parser.zip "%targetFolder%"

cd C:\Dev\Surpath\HL7ParserService\bin\Debug
7z a -tzip %tagVersion%HL7Service.zip * -x!*.config
move %tagVersion%HL7Service.zip "%targetFolder%"

cd c:\dev\surpath