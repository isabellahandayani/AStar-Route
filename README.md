# Implementasi Algoritma A* untuk Menentukan Lintasan Terpendek
> Tugas Kecil III Mata Kuliah IF2211 Strategi Algoritma Tahun Akademik 2020/2021

## Table of contents
* [General info](#general-info)
* [Documentation](#documentation)
* [Requirements](#requirements)
* [Setup](#setup)
* [How To Run](#how-to-run)
* [Author](#author)

## General info
Algoritma A* termasuk algoritma informed search karena terdapat heuristic atau informasi tambahan yang memperkirakan cost hingga simpul tujuan. Algoritma ini dapat digunakan untuk menentukan lintasan terpendek dari suatu titik ke titik lain. Ide dari algoritma A* adalah menghindari melakukan expand pada node yang “mahal”. Heuristic pada algoritma A* akan melakukan estimasi nilai dari node
Heuristic Search
Adapun fungsi evaluasi heuristic pada algoritma A* dapat dinyatakan dengan
```
f(n) = g(n) + h(n)
```

Keterangan :
g(n) = cost so far to reach n
h(n) = estimated cost from n to goal
f(n)  = estimated total cost of path through n to goal

Dalam penentuan rute terpendek, nilai h(n) yang digunakan berdasarkan straight-line distance dari n ke goal. Akan dilakukan perhitungan nilai h(n) menggunakan haversine.

## Documentation
<img width="960" alt="back" src="https://user-images.githubusercontent.com/63598464/113813187-9f19f100-9799-11eb-9eff-a86163beb664.png">
<img width="960" alt="front1" src="https://user-images.githubusercontent.com/63598464/113813189-a0e3b480-9799-11eb-8400-296713ce4b77.png">

## Requirements
1. Visual Studio
2. Microsoft MSAGL Library

## Setup
Lakukan _clone_ pada repository
```
git clone https://github.com/isabellahandayani/AStar-Route
```

## How To Run
### Alternatif 1
Buka folder bin dan run AStar-Route.exe

### Alternatif 2
Jika file exe tidak dapat dijalankan,
1. Buka kakas visual studio
2. _Open_ file sln milik projek
3. Start

## Author
Isabella Handayani Sumantri - 13519081
