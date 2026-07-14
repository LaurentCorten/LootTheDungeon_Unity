# LootTheDungeon — Etat du projet & plan d'action

## Architecture en place
```
CombatManager (orchestre le flux global)
├── CombatResolver (calculs purs, non-MonoBehaviour, instanciable)
└── CombatSequencer (animations et timing uniquement)
```

**Scripts stables :**
- `Fighter.cs`, `Hero.cs`, `Enemy.cs`, `EnemyData.cs`, `HeroData.cs` — architecture trois couches (classe pure / ScriptableObject / asset)
- `AttackResultDto.cs` — readonly struct (C# 9, pas record struct)
- `CombatResolver.cs` — RollInit / RollTouch / RollDmg / ResolveAttack
- `CombatVisual.cs` — bump transform + Animator + flag `_impactReached` + `ReceiveAttackImpact()` via Animation Event + fallback null-animator
- `CombatSequencer.cs` — PlayAttack / PlayReaction / PlayDeath / ResetVisuals, keye par `bool isHero`
- `CombatManager.cs` — orchestre la boucle do/while, garde le Resolver, delegue les visuels au Sequencer, gere les cameras Cinemachine via Priority (candidat futur `SceneFlowManager`)
- `GridManager.cs`, `SpawnManager.cs`, `PlayerMovement.cs`, `PlayerState.cs`, `GameManager.cs`, `FadeManager.cs`, `Tile.cs`, `TileLabeller.cs` — stables

**Assets visuels :**
- Skeleton (SazenGames) — Animator Controller configure (Idle/Attack/Hit/Death), animation `Skeleton_throw_projectiles` retenue pour l'attaque, Animation Event `ReceiveAttackImpact` place sur le bon frame via FBX Import Settings
- RPGHero — pack retenu pour le hero, textures reassignees manuellement (`PolyArtTexture`, `HandPaintedTexture`, `PBR_Albedo`)

**Son :**
- Bande son et effets sonores sommaires en place, synchro et balance a retravailler + a completer mais priorite au HUD.

**Decisions de design figees :**
- Mort simultanee possible, les deux PlayDeath se jouent apres la boucle complete
- HP persistants sans soin entre les combats
- Cameras gerees par Priority Cinemachine (cmCombat a 20 en combat, 0 en exploration)
- `CombatResolver` instanciable (non-static) pour testabilite future

---

## Plan d'action

**Etape suivante — UI barres de vie**
- Barre de vie Hero (HUD permanent, en haut a gauche selon le GDD)
- Barre de vie Ennemi (visible uniquement en phase de combat, en haut a droite en miroir)
- Mise a jour en temps reel au moment de l'impact (brancher sur les resultats des `AttackResultDto`)

**Etape suivante+1 — Floating Combat Text**
- Texte affichant les degats au moment de l'impact
- Monte puis disparait progressivement (tween)
- S'appuie sur `TextMeshProUGUI`

**Etape suivante+2 — Ambiance sonore partie 2**
- Synchroniser les effets sonores et ajuster les balances
- Musique de combat
- Son de mort et victoire
- Musique/son ambiance ecran menu 

**Dans le backlog :**
- Randomisation des clips d'attaque (4 clips Skeleton disponibles : slash01, slash02, stab, throw_projectiles) — option B retenue (tableau `string[]` en SerializeField dans CombatVisual)
- `SceneFlowManager` ou `CameraManager` pour extraire la gestion camera de `CombatManager`
- Retravail de la bande son et des effets sonores
- Randomization de la musique d'ambiance


## Style d'echange a respecter

Approche pedagogique, questions de clarification avant d'agir, scripts complets fournis en un bloc (pas de co-ecriture ligne par ligne), pas d'accents (preference utilisateur), pas de generation de document sauf demande explicite mais fournir le contenu en texte avec marqueurs markdown non interpretes pour copier-coller.