function copyDlls {
    param (
        
        )
        $basepath = "C:\\Origin-OrderManagementWarehouse\\Origin-OrderWarehouse\\Release3.0\\";
        $debugPath = "bin\\Debug\\netcoreapp3.1\\";
        $frameworkPath = "Framework\\";
        $fwprojs = @("Maersk.Framework.Common","Maersk.Framework.Logger","Maersk.Framework.Repository");
        $svcType = "Warehouse";
        $exclude = @(".sonarlint",".vs",".vscode",".git");

        [string[]]$files = @();

        $fwprojs | ForEach-Object {
            $files+=$basepath+$frameworkPath+$_+$debugPath+$_+".dll";
            $files+=$basepath+$frameworkPath+$_+$debugPath+$_+".pdb";
            $files+=$basepath+$frameworkPath+$_+$debugPath+$_+".deps";
            #echo $basepath$frameworkPath$_$debugPath$_".dll"
            #echo $basepath$frameworkPath$_$debugPath$_".pdb"
            #echo $basepath$frameworkPath$_$debugPath$_".deps"
        }
     $files | ForEach-Object {echo $_};
 }