# 🧠 Caso Práctico 3 – ETL y EDA del dataset OULAD

Maestría en Ciencia de Datos e Inteligencia Artificial (MACDIA)
Materia: Ciencia de Datos I – INF-7303-C1
Profesor: Silverio Del Orbe A.

## 🎯 Objetivo del proyecto
Este repositorio corresponde al tercer caso práctico colaborativo. El objetivo es
exportar el modelo del dataset **OULAD** a **SQL Server**, limpiar la data,
crear las claves primarias y foráneas, generar campos ordinales para variables
categóricas y preparar un `FullDomain` en los niveles detallados de **ASSESS** y
**VLE**. Sobre la base resultante se ejecuta un proceso de **ETL** y un **EDA**
extendido implementado preferiblemente en C#.

## 🧱 Arquitectura de la solución
La solución sigue los principios de *Clean Architecture* y separa la lógica en
capas claras:

- **DataAccess** – DbContext de Entity Framework Core y scripts de migración.
- **Domain** – Entidades con validaciones y reglas de negocio.
- **DataImport** – Lectura de CSV y mapeo de variables categóricas a ordinales.
- **Infrastructure** – Utilidades de logging y carga masiva de datos.
- **Pipeline** – Implementación del proceso ETL orquestado.
- **Tests** – Conjunto de pruebas unitarias para validadores y mapeadores.
- **Eda** – `ExtendedEda` genera matrices de confusión y correlación, además de
  boxplots, histogramas y gráficas de dispersión usando OxyPlot.

## 🧰 Stack Tecnológico

| Capa            | Tecnología                       |
|-----------------|----------------------------------|
| Backend         | .NET 9.0 (consola)               |
| ORM             | Entity Framework Core            |
| Base de Datos   | SQL Server                       |
| EDA             | C# (.NET), ML.NET, OxyPlot       |
| IDE             | Visual Studio 2022 / VS Code     |

## 📂 Obtención del dataset

El proyecto no incluye los archivos CSV de **OULAD**. Puedes descargarlos de la
[página oficial del Open University Learning Analytics Dataset](https://analyse.kmi.open.ac.uk/open_dataset/).
Descomprime el contenido en una carpeta local y especifica esa ruta al ejecutar
el modo ETL.

<img src="https://raw.githubusercontent.com/Blanquitoh/OULAD-DSC/refs/heads/master/src/Images/Database-schema-of-OULAD-dataset-online-available-at.png" alt="Schema" width="425" height="400"/>

## ✅ Criterios de evaluación cubiertos

| Criterio                                                     | Cumplimiento |
|--------------------------------------------------------------|--------------|
| Montar el OULAD en un DBMS                                   | ✅ Script DDL |
| ETL orquestado y utilidades encapsuladas                     | ✅ Implementado |
| EDA extendido (matrices de confusión, correlaciones, etc.)   | ✅ C# |
| Artículo de divulgación con hallazgos                        | ✅ En elaboración |

## 👥 Equipo de trabajo
- Víctor Martín Blanco Núñez
- Anthony Manuel Burgos Reyes

## 📄 Documentación de entrega
El documento en formato `.docx` incluye una presentación del equipo en estilo
APA, un resumen técnico de 250 palabras, capturas y enlaces al repositorio.

## 🚀 Cómo ejecutar el proyecto
1. Clona este repositorio.
2. Copia `src/appsettings.sample.json` a `src/appsettings.json` y edita la
   cadena de conexión a tu servidor SQL Server.
3. Ejecuta `./setup.sh` para instalar el SDK de .NET si aún no está disponible.
4. Navega a la carpeta `src` y ejecuta `dotnet restore` para descargar las
   dependencias.
5. Desde esa misma ruta corre `dotnet ef database update` para generar el
   esquema. Este comando también aplica la migración `CreateFullDomainView` que
   genera la vista `FullDomain` consolidando las tablas **ASSESS** y **VLE**.
   Si prefieres ejecutarla manualmente puedes correr:
   ```bash
   sqlcmd -S <server> -d <db> -i src/sql/create_full_domain.sql
   ```
6. Corre la aplicación en modo ETL:
   ```bash
   dotnet run -- --mode Etl --csv-dir <ruta-a-csv>
   ```
7. Para el análisis exploratorio ejecuta:
   ```bash
   dotnet run -- --mode Eda
   ```
   Las gráficas generadas por `ExtendedEda` se guardarán en la carpeta `plots`.
8. Ejecuta `./test.sh` para construir y correr todas las pruebas.
9. `Program.cs` acepta los parámetros opcionales `--connection-string` para
   definir la conexión a la base de datos y `--log-level` para ajustar la
   verbosidad de Serilog.

## 📚 Referencias
- [Entity Framework Core Docs](https://learn.microsoft.com/ef/)
- [SQL Server Documentation](https://learn.microsoft.com/sql/)

## 📊 Ejemplos de Resultados EDA:
- ttest: t-statistic: 3.2518
- anova: F-statistic: 83.4006

<p align="center">
  <img src="https://raw.githubusercontent.com/Blanquitoh/OULAD-DSC/refs/heads/master/src/Images/Plots/boxplot.png" alt="Boxplot" width="300" height="200">
  <img src="https://raw.githubusercontent.com/Blanquitoh/OULAD-DSC/refs/heads/master/src/Images/Plots/confusion.png" alt="Confusion" width="300" height="200">
  <img src="https://raw.githubusercontent.com/Blanquitoh/OULAD-DSC/refs/heads/master/src/Images/Plots/correlation.png" alt="Correlation" width="300" height="200">
  <img src="https://raw.githubusercontent.com/Blanquitoh/OULAD-DSC/refs/heads/master/src/Images/Plots/normal.png" alt="Normal" width="300" height="200">
  <img src="https://raw.githubusercontent.com/Blanquitoh/OULAD-DSC/refs/heads/master/src/Images/Plots/scatter.png" alt="Scatter" width="300" height="200">
  <img src="https://raw.githubusercontent.com/Blanquitoh/OULAD-DSC/refs/heads/master/src/Images/Plots/barchart.png" alt="Scatter" width="300" height="200">
</p>

## ⚖️ Licencia y contribuciones
Este proyecto se distribuye bajo la [Licencia MIT](LICENSE). Las contribuciones
se aceptan mediante *issues* y *pull requests* siguiendo las buenas prácticas de
GitHub.
