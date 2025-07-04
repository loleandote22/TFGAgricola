# Consideraciones previas
Este repositorio forma parte del proyecto de fin de grado para [Ingeniería Informática](https://www.uclm.es/es/toledo/fcsociales/grado-informatica) en la [Univesidad de Castilla La Mancha](https://www.uclm.es/).<br/>
Se puede encontrar todo el backend del proyecto contenerizado con [Docker](https://www.docker.com/), desde la base de datos, la API y los intermediarios que exponen la API al exterior desde una conexión que no dispone de un IP fija.<br/>
Un elemento a tener en cuenta es que se ha priorizado el uso de herramientas y frameworks diseñados o basados en soluciones de Microsft con la finalidad de mejorar la comunicación entre componentes.<br/>
# Estructura del frontend
Consiste en una solución implementada en el framework [uno platform](https://platform.uno/).<br/>
Este permite desarrollar aplicaciones multiplataforma y web reutilizando el código. Existen alternativas como [.NET MAUI](https://dotnet.microsoft.com/en-us/apps/maui), [AvaloniaUI](https://avaloniaui.net/)...
## ¿Por qué unoplatform?
### unoplatform vs .NET MAUI
El principal motivo de la elección de este framework sobre .NET MAUI es que unoplatform permite desarrollar con los contenidos de MAUI por lo cual puedes hacer distinción por tipo de dispositivo dentro del xaml, pero solo entre Windows, Mac, Android e Ios, uno platform permite ejecutar la aplicación ademas en linux y Web.
### unoplatform vs Avalonia UI
El detonante de optar por unoplatform fue que para poder haceer que la interfaz de una página se adapte al tamaño de la ventana hay que hacer uso de código de terceros lo cual puede suponer una vulnerabilidad, mientras que unoplatform si lo tiene incluido.
### Test de interfaz
Unoplatform permite crear pruebas de interfaz automatizadas con las cuales se puede probar el comportamiento de los elementos de la pantalla del mismo modo que lo haría un usuario [Uno.UITest](https://platform.uno/docs/articles/external/uno.uitest/doc/using-uno-uitest.html).
# Código del backend
El código que gestiona los datos de la aplicación está en el repositorio [ApiTFG](https://github.com/loleandote22/ApiTFG).
