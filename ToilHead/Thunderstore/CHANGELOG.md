# v1.9.1

- Updated description

## v1.9.0

- Recompiled the mod for Lethal Company v81

## v1.8.0

- Updated to v73+

## v1.7.2

- Turret-Head players can no longer spawn at the Company building or when playing alone.

## v1.7.1

- Fixed an issue in v55

## v1.7.0

- Added Toil-Masked.
- Added Slayer-Masked.
- Added support for the [RandomEnemiesSize](https://thunderstore.io/c/lethal-company/p/Wexop/RandomEnemiesSize/) mod.
- Added support for the [SCP173CoilheadSFX](https://thunderstore.io/c/lethal-company/p/raydenoir/SCP173CoilheadSFX/) Coil-Head reskin mod.
- Added support for the [WeepingAngels](https://thunderstore.io/c/lethal-company/p/raydenoir/WeepingAngels/) Coil-Head reskin mod.
- Added support for the [FNaFEndoCoilhead](https://thunderstore.io/c/lethal-company/p/OrtonLongGaming/FNaFEndoCoilhead/) Coil-Head reskin mod.
- Added support for the [ThiccCoilHead](https://thunderstore.io/c/lethal-company/p/CommanderJoJo/ThiccCoilHead/) Coil-Head reskin mod.
- Added support for the [A Rather Silly Coil Head](https://thunderstore.io/c/lethal-company/p/COREsEND/A_Rather_Silly_Coil_Head/) Coil-Head reskin mod.
- Made some changes to the [API](https://github.com/ZehsTeam/Lethal-Company-ToilHead/blob/master/ToilHead/Api.cs).
- Other changes.

## v1.6.3

- Fixed some Turret-Head turrets sometimes not properly despawning.
- Small changes.

## v1.6.2

- Changed some config settings default values.

## v1.6.1

- Reduced Turret and Minigun damage.
- Added Manti-Slayer
- Added Toil-Player
- Added Slayer-Player
- Added more properties to the [API](https://github.com/ZehsTeam/Lethal-Company-ToilHead/blob/master/ToilHead/Api.cs).

## v1.6.0

- Reworked all the configs and added some new ones.
- Made some changes to the [API](https://github.com/ZehsTeam/Lethal-Company-ToilHead/blob/master/ToilHead/Api.cs).

<details>
  <summary>Older Versions</summary>

## v1.5.1

- Moved the moon [Toilation](https://thunderstore.io/c/lethal-company/p/Zehs/Toilation/) to a separate mod.
- Fixed Manti-Toil turrets not despawning after the Manticoil despawns.
- Fixed Manti-Toil config settings not working properly.
- Added `ToilSlayerMaxSpawnCount` config setting.
    - *Description: Toil-Slayer max spawn count.*
- Added `ToilSlayerSpawnChance` config setting.
    - *Description: The percent chance a Coil-Head turns into a Toil-Slayer.*
- Added Toil-Slayer properties to the [API](https://github.com/ZehsTeam/Lethal-Company-ToilHead/blob/master/ToilHead/Api.cs).

## v1.5.0

- Added a new moon called [Toilation](https://thunderstore.io/c/lethal-company/p/Zehs/Toilation/).

## v1.4.3

- Added Toil-Slayer.

## v1.4.2

- Added `EnableConfiguration` config setting.
    - *Description: Enable if you want to use custom set config setting values. If disabled, the default config setting values will be used.*
- Changed `PlushieCarryWeight` config setting default value from 4 to 6
- Changed `PlushieMinValue` config setting default value from 150 to 80

## v1.4.1

- Changed Manti-Toil line of sight.
    - Decreased view distance from 50 to 30 meters.
    - Decreased horizontal view range from 40 to 30 degrees.
    - Increased vertical view range.
- Added more LevelTypes to the `PlushieMoonSpawnList` config setting.

## v1.4.0

- Added Manti-Toils
    - *Description: Manticoils can sometimes spawn with a turret on their head.*
- Added `MantiToilMaxSpawnCount` config setting.
    - *Description: Manti-Toil max spawn count.*
- Added `MantiToilSpawnChance` config setting.
    - *Description: The percent chance a Manticoil turns into a Manti-Toil.*

## v1.3.7

- Added `PlushieSpawnAllMoons` config setting.
    - *Description: If true, the Toil-Head plushie will spawn on all moons. If false, the Toil-Head plushie will only spawn on moons set in the moons list.*
- Added `PlushieMoonSpawnList` config setting.
    - *Description: The list of moons the Toil-Head plushie will spawn on. (Experimentation, Assurance, Vow, Offense, March, Adamance, Rend, Dine, Titan, Artifice, Embrion) Only works if PlushieSpawnAllMoons is false.*
- Added `PlushieCarryWeight` config setting.
    - *Description: Toil-Head plushie carry weight in pounds.*
- Added `PlushieMinValue` config setting.
    - *Description: Toil-Head plushie min scrap value.*
- Added `PlushieMaxValue` config setting.
    - *Description: Toil-Head plushie max scrap value.*

## v1.3.6

- Added `PlushieSpawnWeight` config setting.
    - *Description: Toil-Head plushie spawn chance weight. (Higher = more common)*
- Updated [Asteroid13](https://thunderstore.io/c/lethal-company/p/Magic_Wesley/Asteroid13/) secrets.

## v1.3.5

- Updated `CustomSpawnSettings` config setting default value and formatting.

## v1.3.4

- Added [Asteroid13](https://thunderstore.io/c/lethal-company/p/Magic_Wesley/Asteroid13/) secrets back in.
- Added a Toil-Head monster plushie when you have the [Monster Plushies](https://thunderstore.io/c/lethal-company/p/Scintesto/Monster_Plushies/) mod installed.

## v1.3.3

- Added `RealToilHeadPlayerRagdolls` config setting.
    - *Description: If enabled, will spawn a real turret on the Toil-Head player ragdoll.*
- Added `CustomSpawnSettings` config setting.
    - *Description: Toil-Head spawn settings for modded moons. You can now specify any modded moon's Toil-Head MaxSpawnCount and SpawnChance.*
- Added `ExperimentationSpawnSettings` config setting.
    - *Description: Toil-Head spawn settings for 41-Experimentation*
- Added `AssuranceSpawnSettings` config setting.
    - *Description: Toil-Head spawn settings for 220-Assurance*
- Decreased `TurretCodeAccessCooldownDuration` from 10 to 7
- Increased `AdamanceSpawnSettings` `SpawnChance` from 25 to 30
- Increased `TitanSpawnSettings` `MaxSpawnCount` from 1 to 2
- Increased `ArtificeSpawnSettings` `MaxSpawnCount` from 1 to 2
- Added XML file for [API](https://github.com/ZehsTeam/Lethal-Company-ToilHead/blob/master/ToilHead/Api.cs) documentation.

## v1.3.2

- Added `forceMaxSpawnCount` int property to the [API](https://github.com/ZehsTeam/Lethal-Company-ToilHead/blob/master/ToilHead/Api.cs).
    - *Description: If set to any value above -1, will temporarily override the Toil-Head max spawn count for the day. This will get reset automatically when the day ends.*
- Changed `forceToilHeadSpawns` bool property in the [API](https://github.com/ZehsTeam/Lethal-Company-ToilHead/blob/master/ToilHead/Api.cs) to `forceSpawns`.

## v1.3.1

- Added `SpawnToilHeadPlayerRagdolls` config setting.
    - *Description: If enabled, will spawn a Toil-Head player ragdoll when a player dies to a Toil-Head in any way.*
- Increased `DineSpawnSettings` `SpawnChance` from 30 to 45.
- Added `forceToilHeadSpawns` boolean property to the [API](https://github.com/ZehsTeam/Lethal-Company-ToilHead/blob/master/ToilHead/Api.cs).
    - *Description: If enabled, will force any spawned Coil-Heads to become Toil-Heads. This will get reset automatically when the day ends.*

## v1.3.0

- Replaced all Toil-Head config settings with new per moon config settings.
- Updated all turret config settings keys.
- Added `ExtendedLogging` config setting.
- Improved random percent calculations.

## v1.2.3

- Changed `spawnChance` config setting default value to 40
- Added `useAdditionalSpawnChance` config setting.
    - *Description: The dynamic additional spawn chance for the Toil-Head based on certain circumstances.*

## v1.2.2

- Added [API](https://github.com/ZehsTeam/Lethal-Company-ToilHead/blob/master/ToilHead/Api.cs).

## v1.2.1

- Improved line of sight logic.
- Renamed `turretRotationWhenSearching` config setting to `turretDetectionRotation`.
- Added `turretLostLOSDuration` config setting.
   - *Description: The duration until the turret loses the target player when not in line of sight.*
- Added `turretChargingDuration` config setting.
   - *Description: The duration of the turret charging state.*
- Added `turretFiringRotationSpeed` config setting.
   - *Description: The rotation speed of the turret when in firing state.*
- Added `turretBerserkDuration` config setting.
   - *Description: The duration of the turret berserk state.*
- Added `turretBerserkRotationSpeed` config setting.
   - *Description: The rotation speed of the turret when in berserk state.*
- Balanced config settings to be more fair.
- Other changes.

## v1.2.0

- Removed `hideTurretBody` config setting.
- Removed `spawnTurretFacingForwardWeight` config setting.
- Removed `spawnTurretFacingBackwardWeight` config setting.
- Added `turretRotationWhenSearching` config setting.
   - *Description: If enabled, the turret will rotate when searching for players.*
- Added `turretDetectionRotationSpeed` config setting.
   - *Description: The rotation speed of the turret when searching for players.*
- Added `turretChargingRotationSpeed` config setting.
   - *Description: The rotation speed of the turret when charging at the target player.*
- Added `turretRotationRange` config setting.
   - *Description: The rotation range of the turret in degrees.*
- Added `turretCodeAccessCooldownDuration` config setting.
   - *Description: The duration of the turret being disabled from the terminal in seconds.*
- Fixed the radar map graphics for the turret.
- Fixed the turret code radar map graphic to follow the Toil-Head.
- Hitting the Toil-Head will now trigger the turret berserk state.
- Disabled [Asteroid13](https://thunderstore.io/c/lethal-company/p/Magic_Wesley/Asteroid13/) secrets until the moon supports version 50.
- Other changes.

## v1.1.0

- Tested and working in version 50 and version 49.
- Renamed `maxSpawns` config setting to `maxSpawnCount`.
- Added `hideTurretBody` config setting.
- Moved all config settings to new categories.
- Changed `spawnChance` and `maxSpawnCount` config setting default values.
- Fixed Toil-Head turrets not despawning when the Toil-Head despawns.

## v1.0.8

- Hopefully fixed an issue where Toil-Head turrets aren't despawning properly when you leave the moon.

## v1.0.7

- Updated [Asteroid13](https://thunderstore.io/c/lethal-company/p/Magic_Wesley/Asteroid13/) secrets.

## v1.0.6

- Fixed [Asteroid13](https://thunderstore.io/c/lethal-company/p/Magic_Wesley/Asteroid13/) secrets not working sometimes.

## v1.0.5

- Small config changes.
- Updated secrets.

## v1.0.4

- Added secrets to [Asteroid13](https://thunderstore.io/c/lethal-company/p/Magic_Wesley/Asteroid13/).

## v1.0.3

- Fixed Toil-Head spawning from incorrect seed.
- Fixed max Toil-Head spawns.

## v1.0.2

- Turrets can now spawn facing forward or backward depending on the spawn chance weight.
- Added `spawnTurretFacingForwardWeight` and `spawnTurretFacingBackwardWeight` config settings.

## v1.0.1

- Changed config settings default values.
- Updated README.

## v1.0.0

- Initial release.
