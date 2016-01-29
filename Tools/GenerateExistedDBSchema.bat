RD ..\TestData\DatabaseSchemaTestModel\
MD ..\TestData\DatabaseSchemaTestModel\
"%windir%\Microsoft.NET\Framework\v4.0.30319\edmgen.exe" /mode:FullGeneration ^
            /c:"Data Source=STEVEN-PC\SQLEXPRESS; Initial Catalog=YN_Test; Integrated Security=SSPI" ^
            /project:..\TestData\DatabaseSchemaTestModel\DatabaseSchemaTestModel ^
            /entitycontainer:TestEntities ^
            /namespace:DatabaseSchemaTestModelNM^
            /language:CSharp
