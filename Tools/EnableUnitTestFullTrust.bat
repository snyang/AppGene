::"%windir%\Microsoft.NET\Framework\v2.0.50727\caspol" -m -ag 1.2 -url file:///D:/* FullTrust
%windir%\Microsoft.NET\Framework\v4.0.30319\CasPol -m -ag 1.2 -url file://E:/* FullTrust
SETX COMPLUS_LoadFromRemoteSources 1 /M
Echo Please restart your Visual Sutdio...