# CellWar.Game
~~See [TODO List](<https://github.com/bennycui99/Cellwar.Game/blob/master/TODO.md>) and fuck your tasks off! LOL~~

See [issue](https://github.com/bennycui99/Cellwar.Game/issues) page and fuck all issues off please.

## Where the hell we are?

### 9.24 Internal Gene 

我们必须知道我们所说的玩家不能使用的gene仅仅是在lab中不可以使用。

如果该基因为内部基因，则需要填写为形如，

```json
[
    ...
    {
        ...
        "IsInternal": "true",
        ...
    }
    ...
]
```

默认不填写时为false。即在lab中可见。



### 9.15 Fix all effect bugs and some descriptions



#### Essential Items（必填项）

```json
{
    "Name": "No_Consume",
    "Length": "0",
    "PopulationCoefficient": "0",
    "PopulationIntercept": "0",
    "FirstSpreadMountRate": "1",
    "SpreadConditionRate": "1",
    "Description": "fooooooooooooooooo"
  }
```

The gene above is the minimal json version of coding gene. The properties given above are essential.

```json
    "FirstSpreadMountRate": "1",
```

The properties above must be both sans-zero.

```json
    "SpreadConditionRate": "0",
```

Means always spreading.

 

#### The Main Key of xxxxxxx（某一指标的主键）

The model of the coding gene contains several properties like **xxxxxxxName** and we named those string type properties **The Main Key of xxxxxxxx**.

In the [effect methods](https://github.com/bennycui99/Cellwar.Game/blob/master/CellWar.Game/Assets/Scripts/Controller/GeneController.cs) written so far, if **The Main Key of xxxxxxxx** is **Empty** ("") or **Null Reference**, the method will not be invoked.

e.g.

```json
  {
    "Name": "No_Consume",
    "Length": "0",
    "PopulationCoefficient": "0",
    "PopulationIntercept": "0",
    "ConsumeChemicalName": "",
    "ConsumeChemicalCoeffeicient": "0",
    "ConsumeChemicalIntercept": "0",
    "IsConsumePublic": "False",
    "ProductionChemicalName": "Demo",
    "ProductionChemicalCoeffeicient": "1",
    "ProductionChemicalIntercept": "2",
    "ImportChemicalName": "Demo",
    "ImportChemicalCoeffeicient": "1",
    "ImportChemicalIntercept": "2",
    "FirstSpreadMountRate": "1",
    "SpreadConditionRate": "1",
    "Description": "fooooooooooooooooo"
  }
```

The gene above will never call the consume method.



### 9.10 In-Game Ready to use panel animation (in a separated project)

![avatar](Progress/9.10.gif)

Once the gameplay itself is fixed, we can begin to refine its looking.

### 9.8 MapEditor
When holding a bacteria/ chemical you can not active/de-active a block.

Left Click to active/ right click to de-active.

Left Click multiple times can change the capacity of a block.

Left Click to add to block(can add multiple tiems)/Right Click to discard the holding.

Z to discard last bacteria/ X to discard last chemical.

Bacteria reads from Resources/Save/strain.json; Chemicals reads from Resources/GameData/chemicals.json.

Export to Resources/Save/map_generation.json

GameScene reads map from Resources/GameData/map.json

### 9.6 Implement Video Manager - Fade between scenes

The in-game fade in animation remain unimplemented, but the API is easy to use.
![avatar](Progress/9.6.gif)


### 9.4 Smooth Game Animations

![avatar](Progress/9.4.gif)



### 7.30 Finished Lab

![avatar](Progress/7.30.gif)

## About CellWar Json Files In GameData

We use excel to edit raw data and for 2json, you can use [THIS SITE](<http://www.bejson.com/json/col2json/>)

to convert the excel to json file.

see **CellWar.Document/game_data.xlsx** file to make sense about the core game data of CW.



## Directories

The repository contains following folders in root directory.

* **CellWar.Game** - Unity3d project.

  

## FAQs

### Haste cloning & pulling

For the fucking previous commits were not using gitignore file, it might takes you a fucking shit long time to clone the whole repository. So you can clone like this.

#### Clone

~~~shell
$ git clone --depth=1 https://github.com/bennycui99/iGEM-game.git
~~~

Or you want pull the latest change.

#### Pull

```shell
$ git pull --depth=1
```

Or a lazier way.

```shell
$ . pull.sh
```



### Haste Unity Starting Up

You should install [ppbash](<http://github.com/cyf-gh/ppbash>) first enable to use the [go] command.

```shell
$ . open_scene.sh
```

For powershell,

```powershell
PS >. open_scene.ps1
```
