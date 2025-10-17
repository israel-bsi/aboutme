# Garante que todos os arquivos .resx tenham encoding UTF-8 com BOM
Get-ChildItem -Path "Resources" -Recurse -Filter "*.resx" | ForEach-Object {
    $content = Get-Content $_.FullName -Raw -Encoding UTF8
    $Utf8BomEncoding = New-Object System.Text.UTF8Encoding $true
    [System.IO.File]::WriteAllText($_.FullName, $content, $Utf8BomEncoding)
    Write-Host "Fixed encoding for: $($_.Name)"
}
