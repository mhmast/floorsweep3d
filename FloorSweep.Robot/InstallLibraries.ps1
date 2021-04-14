$loc = Join-Path -Path (Get-Location) -ChildPath "Lib"
foreach($lib in Get-ChildItem -Path $loc -Directory){
    new-item -itemtype symboliclink -path "C:\Program Files (x86)\Arduino\libraries" -name "Floorsweep.$lib" -value (Join-Path -Path $loc -ChildPath $lib) 
}