# 🗡️ Loot The Dungeon

> *Un dungeon crawler minimaliste où chaque combat est une décision de ressources irréversible.*

Prototype Unity v0.1 — PC — Solo

---

## 🎮 Concept

**Loot The Dungeon** est un dungeon crawler first-person, tour par tour, roguelite.

Le joueur incarne un Héros piégé dans une grotte médiévale envahie de créatures hostiles. Pas de carte, pas d'allié, pas de soin possible : il n'a qu'un objectif, trouver la sortie avant d'y laisser sa vie.

Chaque run est générée procéduralement, dure moins de 10 minutes, et se termine soit par la victoire soit par la mort permanente.

---

## 🕹️ Gameplay

### Exploration

La carte est une grille **10×10** générée aléatoirement à chaque partie :

| Case | Description |
|------|-------------|
| 🟢 Départ | Position initiale du Héros |
| 🚪 Sortie | Objectif de la run |
| 🧱 Pilier / Mur | Case infranchissable |
| ⚔️ Encounter | Déclenche un combat à l'entrée |

La génération garantit qu'il existe toujours un chemin praticable entre le départ et la sortie.

Le déplacement est **case par case**, orienté selon les quatre points cardinaux.

```
Z         — Avancer
S         — Reculer
Q / D     — Se déplacer latéralement
A / E     — Pivoter à gauche / droite
```

### Combat

Automatique et tour par tour, inspiré de **D&D 5e** :

1. **Initiative** — jet D20 + DEX pour chaque combattant, le plus haut attaque en premier
2. **Attaque** — jet D20 + bonus ATT vs Classe d'Armure de la cible
3. **Dégâts** — jet xDy si l'attaque touche
4. Le combat se termine quand un combattant tombe à **0 HP**

> ⚠️ Il n'existe aucun moyen de récupérer des HP. Chaque combat est une dépense permanente.

---

## 👾 Personnages

### Le Héros
Stats fixes pour ce prototype : **CON · STR · DEX · INT**, HP max, Classe d'Armure, dés de dégâts.

### L'ennemi : Kobold
Un seul type d'ennemi pour ce prototype. Chétif, rapide, sournois. Stats fixes.

---

## 🎨 Direction artistique

- **Vue labyrinthe** — first-person 3D, éclairage dynamique centré sur la torche du Héros (halo chaud, portée limitée). Les monstres ne sont jamais visibles dans le labyrinthe. La sortie se signale par un halo de lumière blanche en ligne droite.
- **Vue combat** — transition vers une scène dédiée en side-view 2D stylisé. Log textuel des jets en temps réel.
- **Ambiance sonore** — dark fantasy oppressante : nappes ambiantes, pierre, eau, percussions au combat.
- **Game Over** — fade out vers noir, *GAME OVER* en lettres de sang.
- **Victory** — fade out vers blanc, *VICTORY* en lettres d'or.

---

## 🖥️ Interface

| Zone | Contenu | Affiché |
|------|---------|---------|
| Haut gauche | Avatar + barre de vie du Héros | Toujours |
| Haut droite | Avatar + barre de vie du Monstre | Combat uniquement |
| Bas gauche | Rappel des touches | Exploration uniquement |
| Bas droite | Score | Toujours *(actif dès l'itération Scoring)* |

Pas d'écran titre. Pas de menu pause. Au lancement, le joueur est directement en run.

---

## 🗺️ Roadmap

### 🔜 Proto v0.1 — Noyau fonctionnel
- [x] Génération procédurale de la carte 10×10 avec garantie de chemin
- [x] Déplacement first-person grid-based (ZQSDAE)
- [ ] Système de combat automatique tour par tour (D&D-inspired)
- [ ] Héros à stats fixes, un seul ennemi type (Kobold)
- [ ] HUD minimal, écrans de fin (Game Over / Victory)

### 🏆 Itération 1 — Scoring & Loot
- [ ] Points flat pour avoir atteint la sortie
- [ ] Points par Kobold éliminé
- [ ] Coffres à valeur variable visibles à distance sur la carte

### 🔮 Itération 2 — Progression verticale
- [ ] Système d'étages (relance de la boucle complète)
- [ ] Difficulté croissante par étage
- [ ] Armes trouvables dans les coffres

---

## 🛠️ Stack technique

| | |
|--|--|
| Engine | Unity (C#) |
| Support | PC |
| Genre | Dungeon Crawler · Tour par tour · Roguelite |

---

## 📄 Documentation

- `LootTheDungeon_Proto-OnePager_v01.docx` — Document de cadrage
- `LootTheDungeon_Proto-TenPager_v01.docx` — Ten-Pager

---

*Document de travail — Mai 2026*
