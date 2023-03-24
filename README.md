<img
  src="assets/app-preview.png"
  alt="Treasure Hunt App by Bebas"
  style="display: inline-block; margin: 0 auto; width: 100%; max-width:800; padding: 0;">

# Pengaplikasian Algoritma BFS dan DFS dalam Menyelesaikan Persoalan Maze Treasure Hunt

Dalam tugas besar ini, Anda akan diminta untuk membangun sebuah aplikasi dengan GUI sederhana yang dapat mengimplementasikan BFS dan DFS untuk mendapatkan rute memperoleh seluruh treasure atau harta karun yang ada. Program dapat menerima dan membaca input sebuah file txt yang berisi maze yang akan ditemukan solusi rute mendapatkan treasure-nya. Untuk mempermudah, batasan dari input maze cukup berbentuk segi-empat dengan spesifikasi simbol sebagai berikut :

- K : Krusty Krab (Titik awal)
- T : Treasure
- R : Grid yang mungkin diakses / sebuah lintasan
- X : Grid halangan yang tidak dapat diakses

Dengan memanfaatkan algoritma Breadth First Search (BFS) dan Depth First Search (DFS), anda dapat menelusuri grid (simpul) yang mungkin dikunjungi hingga ditemukan rute solusi, baik secara melebar ataupun mendalam bergantung alternatif algoritma yang dipilih. Rute solusi adalah rute yang memperoleh seluruh treasure pada maze. Perhatikan bahwa rute yang diperoleh dengan algoritma BFS dan DFS dapat berbeda, dan banyak langkah yang dibutuhkan pun menjadi berbeda. Prioritas arah simpul yang dibangkitkan dibebaskan asalkan ditulis di laporan ataupun readme, semisal LRUD (left right up down). Tidak ada pergerakan secara diagonal. Anda juga diminta untuk memvisualisasikan input txt tersebut menjadi suatu grid maze serta hasil pencarian rute solusinya. Cara visualisasi grid dibebaskan, sebagai contoh dalam bentuk matriks yang ditampilkan dalam GUI dengan keterangan berupa teks atau warna. Pemilihan warna dan maknanya dibebaskan ke masing - masing kelompok, asalkan dijelaskan di readme / laporan.

Daftar input maze akan dikemas dalam sebuah folder yang dinamakan test dan terkandung dalam repository program. Folder tersebut akan setara kedudukannya dengan folder src dan doc (struktur folder repository akan dijelaskan lebih lanjut di bagian bawah spesifikasi tubes). Cara input maze boleh langsung input file atau dengan textfield sehingga pengguna dapat mengetik nama maze yang diinginkan. Apabila dengan textfield, harus menghandle kasus apabila tidak ditemukan dengan nama file tersebut.

Setelah program melakukan pembacaan input, program akan memvisualisasikan gridnya terlebih dahulu tanpa pemberian rute solusi. Hal tersebut dilakukan agar pengguna dapat mengerjakan terlebih dahulu treasure hunt secara manual jika diinginkan. Kemudian, program menyediakan tombol solve untuk mengeksekusi algoritma DFS dan BFS. Setelah tombol diklik, program akan melakukan pemberian warna pada rute solusi.

## Group Members

| NIM      | Name                     |
| -------- | ------------------------ |
| 13521056 | Daniel Egiant Sitanggang |
| 13521092 | Frankie Huang            |
| 13521150 | I Putu Bakta Hari Sudewa |

## About this project

- Solves 2D Maze Treasure Hunt problem with BFS and DFS algorithm
- Step-by-step display of path taken by the algorithm
- GUI display implemented with C# Desktop Application Development
- Input are available via textbox or searching it manually
- Programmed in C# using Visual Studio IDE

## Features

The features below are 100% done and implemented.

- BFS algorithm solution including its TSP counterpart
- DFS algorithm solution including its TSP counterpart
- TSP implemented with brute-force
- Visual steps taken by the algorithm chosen

## Requirements

If you want to recompile your code, make sure the following program has been installed:

- .NET Framework
- .NET Desktop Development
- Visual Studio (optional)

Otherwise, you can just run the application described in the step below.

## Setup

Make sure all requirements are installed in your local machine beforehand. Then, clone this repository.

```bash
git clone git@github.com:sozyGithub/Tubes2_bebas.git
```

Change to the repository directory, then run the `bin/src.exe` file.

```bash
cd Tubes2_bebas/
bin/src.exe
```

## Program Structure

```
├── assets
│   └── app-preview.png
├── bin
│   ├── src.deps.json
│   ├── src.dll
│   ├── treasurehuntsolver.exe
│   ├── src.pdb
│   └── src.runtimeconfig.json
├── doc
│   └── bebas.pdf
├── README.md
├── src
│   ├── Algorithms
│   │   ├── bfs.cs
│   │   ├── dfs.cs
│   │   ├── PathFinder.cs
│   │   └── TravellingSalesman.cs
│   ├── App.xaml
│   ├── App.xaml.cs
│   ├── AssemblyInfo.cs
│   ├── Assets
│   │   ├── Fonts
│   │   │   ├── fa.otf
│   │   │   └── Kanit-Regular.ttf
│   │   ├── Images
│   │   │   └── logo-bebas.png
│   │   └── logo-bebas.ico
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   ├── obj
│   │   ├── project.assets.json
│   │   ├── project.nuget.cache
│   │   ├── src.csproj.nuget.dgspec.json
│   │   ├── src.csproj.nuget.g.props
│   │   └── src.csproj.nuget.g.targets
│   ├── Properties
│   │   └── PublishProfiles
│   │       ├── FolderProfile.pubxml
│   │       └── FolderProfile.pubxml.user
│   ├── src.csproj
│   ├── src.csproj.user
│   ├── src.sln
│   └── Utils
│       ├── maputils.cs
│       └── validate.cs
└── test
    ├── sampel-1.txt
    ├── sampel-2.txt
    ├── sampel-3.txt
    ├── sampel-4.txt
    ├── sampel-5.txt
    ├── tc_1.txt
    ├── tc_2.txt
    ├── tc_3.txt
    ├── tc_4.txt
    ├── tc_5.txt
    ├── tc_6.txt
    └── tc_7.txt
```

## Project Status

This project is _finished_.

_There are no plans to change, add, or optimize the program in the near future._

## Room for Improvement

- Faster TSP implementation (with dynamic programming)
- Stop, Rewind, and Play button available for easier viewing

## Acknowledgements

- This project is spearheaded by the IF2211 Informatics major at Institut Teknologi Bandung, which has been well organized by the IF2211 - 2023 professors and assistants.
- README template by [@flynerdpl](https://www.flynerd.pl/): [README](https://github.com/ritaly/README-cheatsheet)
- Thanks to the reference sources and methods as a basis that have been listed in the relevant parts of the report.

Made with ❤️ by **bebas** team.
