# MeshIO ![Build](https://github.com/DomCr/MeshIO/actions/workflows/csharp.yml/badge.svg) ![License](https://img.shields.io/github/license/DomCr/MeshIO) ![nuget](https://img.shields.io/nuget/v/MeshIO) [![Coverage Status](https://coveralls.io/repos/github/DomCR/MeshIO/badge.svg?branch=master)](https://coveralls.io/github/DomCR/MeshIO?branch=master)

C# library to read/write and modify different 3D formats.

### Features

MeshIO allows to read or create 3D files using .Net and also extract or modify existing content in the files, the main features may be listed as: 

- Read 3D files
- Write 3D files
- Extract or modify the content in the files like: Meshes, Cameras, Materials...
- Math methods to apply geometric transformations.
- Creation tools to generate meshes or primitives.

#### Compatible 3D file formats:

|      | Read-ASCII | Read-Binary | Write-ASCII | Write-Binary |
------ | :-------: | :-------: | :-------: | :-------: |
FBX6000 |    :heavy_check_mark:    |   :heavy_check_mark:     |    :construction:    |    :construction:    |
FBX7000 |    :heavy_check_mark:    |   :heavy_check_mark:     |    :heavy_check_mark:    |    :heavy_check_mark:    |
STL |    :heavy_check_mark:    |   :heavy_check_mark:     |    :heavy_check_mark:    |        |
GLB |        |   :heavy_check_mark:     |        |    :construction:    |
GLTF |    :construction:    |        |    :construction:    |        |
OBJ |    :construction:    |        |    :x:    |        |

The goal of this project is to give full support for all the formats in the table.

Code Example
---

```c#
public static void Main()
{
	Scene scene;
	using (ISceneReader reader = FileFormat.GetReader(test.Path, onNotification))
	{
		scene = reader.Read();
	}
}

// Process a notification from the reader
private static void onNotification(object sender, NotificationEventArgs e)
{
	Console.WriteLine(e.Message);
}
```

Building
---

Before building run:

```console
git submodule update --init --recursive
```

This command will clone the submodules necessary to build the project.