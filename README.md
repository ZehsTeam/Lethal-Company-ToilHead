# ToilHead
**Coil-Heads can sometimes spawn with a turret on their head. Highly Configurable.**

<ins><b>This mod does not add a new enemy. It gives Coil-Heads a chance to spawn with a turret on their head.</b></ins><br>
<ins><b>This mod is for all clients!</b></ins>

## <img src="https://i.imgur.com/TpnrFSH.png" width="20px"> Download

Download [ToilHead](https://thunderstore.io/c/lethal-company/p/Zehs/ToilHead/) on Thunderstore.

## Mod Compatibility
**This mod is compatible with:**
* [Asteroid13](https://thunderstore.io/c/lethal-company/p/Magic_Wesley/Asteroid13/) - *Adds some secrets*
* [CoilHeadStare](https://thunderstore.io/c/lethal-company/p/TwinDimensionalProductions/CoilHeadStare/) - *If you want to die harder :3*
* [ghostCodes](https://thunderstore.io/c/lethal-company/p/darmuh/ghostCodes/) - *Adds extra functionality*
* [Brutal Company Minus](https://thunderstore.io/c/lethal-company/p/DrinkableWater/Brutal_Company_Minus/) - *Adds extra functionality*
* [Monster Plushies](https://thunderstore.io/c/lethal-company/p/Scintesto/Monster_Plushies/) - *Adds another plushie :3*

## Config Settings
<details>
  <summary>Expand</summary>
<br>

* **Toil-Head Settings** are host only.
* **All Turret Settings** are synced with the host.

| General Settings | Setting type | Default value | Description |
| ----------- | ----------- | ----------- | ----------- |
| `ExtendedLogging` | `Boolean` | `false` | Enable extended logging. |

| Toil-Head Settings | Setting type | Default value | Description |
| ----------- | ----------- | ----------- | ----------- |
| `SpawnToilHeadPlayerRagdolls` | `Boolean` | `true` | If enabled, will spawn a Toil-Head player ragdoll when a player dies to a Toil-Head in any way. |
| `RealToilHeadPlayerRagdolls` | `Boolean` | `true` | If enabled, will spawn a real turret on the Toil-Head player ragdoll. |

| Toil-Head Settings | Setting type | Default value | Description |
| ----------- | ----------- | ----------- | ----------- |
|  |  | `PlanetName:MaxSpawnCount:SpawnChance,` |  |
| `CustomSpawnSettings` | `String` | `57 Asteroid-13:2:30,523 Ooblterra:3:80,` | Toil-Head spawn settings for modded moons. |

| Toil-Head Settings | Setting type | Default value | Description |
| ----------- | ----------- | ----------- | ----------- |
|  |  | `MaxSpawnCount,SpawnChance` |  |
| `OtherSpawnSettings` | `String` | `1,30` | Toil-Head default spawn settings for modded moons. |
| `LiquidationSpawnSettings` | `String` | `1,30` | Toil-Head spawn settings for 44-Liquidation. |
| `EmbrionSpawnSettings` | `String` | `1,20` | Toil-Head spawn settings for 5-Embrion. |
| `ArtificeSpawnSettings` | `String` | `2,70` | Toil-Head spawn settings for 68-Artifice. |
| `TitanSpawnSettings` | `String` | `2,50` | Toil-Head spawn settings for 8-Titan. |
| `DineSpawnSettings` | `String` | `1,45` | Toil-Head spawn settings for 7-Dine. |
| `RendSpawnSettings` | `String` | `1,40` | Toil-Head spawn settings for 85-Rend. |
| `AdamanceSpawnSettings` | `String` | `1,30` | Toil-Head spawn settings for 20-Adamance. |
| `MarchSpawnSettings` | `String` | `1,20` | Toil-Head spawn settings for 61-March. |
| `OffenseSpawnSettings` | `String` | `1,20` | Toil-Head spawn settings for 21-Offense. |
| `VowSpawnSettings` | `String` | `1,20` | Toil-Head spawn settings for 56-Vow. |
| `AssuranceSpawnSettings` | `String` | `1,20` | Toil-Head spawn settings for 220-Assurance. |
| `ExperimentationSpawnSettings` | `String` | `1,10` | Toil-Head spawn settings for 41-Experimentation. |

| Manti-Toil Settings | Setting type | Default value | Description |
| ----------- | ----------- | ----------- | ----------- |
| `MantiToilMaxSpawnCount` | `Int32` | `5` | Manti-Toil max spawn count. |
| `MantiToilSpawnChance` | `Int32` | `50` | The percent chance a Manticoil turns into a Manti-Toil. |

| Plushie Settings | Setting type | Default value | Description |
| ----------- | ----------- | ----------- | ----------- |
| `PlushieSpawnWeight` | `Int32` | `10` | Toil-Head plushie spawn chance weight. (Higher = more common) |
| `PlushieSpawnAllMoons` | `Boolean` | `true` | If true, the Toil-Head plushie will spawn on all moons. If false, the Toil-Head plushie will only spawn on moons set in the moons list. |
| `PlushieMoonSpawnList` | `String` | `Experimentation, Assurance, Vow, Offense, March, Rend, Dine, Titan` | The list of moons the Toil-Head plushie will spawn on. |
| `PlushieCarryWeight` | `Int32` | `4` | Toil-Head plushie carry weight in pounds. |
| `PlushieMinValue` | `Int32` | `150` | Toil-Head plushie min scrap value. |
| `PlushieMaxValue` | `Int32` | `250` | Toil-Head plushie max scrap value. |

| Turret Settings | Setting type | Default value | Description |
| ----------- | ----------- | ----------- | ----------- |
| `TurretLostLOSDuration` | `Single` | `0.75` | The duration until the turret loses the target player when not in line of sight. |
| `TurretRotationRange` | `Single` | `75` | The rotation range of the turret in degrees. |
| `TurretCodeAccessCooldownDuration` | `Single` | `7` | The duration of the turret being disabled from the terminal in seconds. |

| Turret Detection Settings | Setting type | Default value | Description |
| ----------- | ----------- | ----------- | ----------- |
| `TurretDetectionRotation` | `Boolean` | `false` | If enabled, the turret will rotate when searching for players. |
| `TurretDetectionRotationSpeed` | `Single` | `28` | The rotation speed of the turret when in detection state. |

| Turret Charging Settings | Setting type | Default value | Description |
| ----------- | ----------- | ----------- | ----------- |
| `TurretChargingDuration` | `Single` | `2` | The duration of the turret charging state. |
| `TurretChargingRotationSpeed` | `Single` | `95` | The rotation speed of the turret when in charging state. |

| Turret Firing Settings | Setting type | Default value | Description |
| ----------- | ----------- | ----------- | ----------- |
| `TurretFiringRotationSpeed` | `Single` | `95` | The rotation speed of the turret when in firing state. |

| Turret Berserk Settings | Setting type | Default value | Description |
| ----------- | ----------- | ----------- | ----------- |
| `TurretBerserkDuration` | `Single` | `9` | The duration of the turret berserk state. |
| `TurretBerserkRotationSpeed` | `Single` | `77` | The rotation speed of the turret when in berserk state. |

</details>

## API
<details>
  <summary>Expand</summary>
<br>

https://github.com/ZehsTeam/Lethal-Company-ToilHead/blob/master/ToilHead/Api.cs
```cs
// This is for all enemy turret pairs.
public static Dictionary<NetworkObject, NetworkObject> enemyTurretPairs { get; }

// Toil-Head spawn count.
public static int spawnCount { get; }

// If enabled, will force any spawned Coil-Heads to become Toil-Heads.
// This will get reset automatically when the day ends.
public static bool forceSpawns { get; set; }

// If set to any value above -1, will temporarily override the Toil-Head max spawn count.
// This will get reset automatically when the day ends.
public static int forceMaxSpawnCount { get; set; }

// This must only be called on the Host/Server.
// Only accepts an EnemyAI instance where the EnemyType.enemyName is "Spring".
// Returns true if successful.
public static bool SetToilHeadOnServer(EnemyAI enemyAI) { }

// Manti-Toil spawn count.
public static int mantiToilSpawnCount { get; }

// If enabled, will force any spawned Manticoils to become Manti-Toils.
// This will get reset automatically when the day ends.
public static bool forceMantiToilSpawns { get; set; }

// If set to any value above -1, will temporarily override the Manti-Toil max spawn count.
// This will get reset automatically when the day ends.
public static int forceMantiToilMaxSpawnCount { get; set; }

// This must only be called on the Host/Server.
// Only accepts an EnemyAI instance where the EnemyType.enemyName is "Manticoil".
// Returns true if successful.
public static bool SetMantiToilOnServer(EnemyAI enemyAI) { }
```

</details>

## Bug Reports, Help, or Suggestions
https://github.com/ZehsTeam/Lethal-Company-ToilHead/issues

| Discord server | Forum | Post |
| ----------- | ----------- | ----------- |
| [Lethal Company modding Discord](https://discord.gg/XeyYqRdRGC) | `#mod-releases` | [ToilHead](https://discord.com/channels/1168655651455639582/1207108508298911834) |
| [Unofficial Lethal Company Community](https://discord.gg/nYcQFEpXfU) | `#mod-releases` | [ToilHead](https://discord.com/channels/1169792572382773318/1207108696589606932) |

## Screenshots
<div>
    <img src="https://i.imgur.com/2wvuDcg.jpeg" width="412px">
    <img src="https://i.imgur.com/dXMbu6m.jpeg" width="412px">
</div>