import 'package:flutter/material.dart';
import 'package:mymangawebsite/model/ChapterList.dart';
import 'package:mymangawebsite/services/DatabaseService.dart';

import 'CardHead.dart';
import 'ChapterButton.dart';

final fixedLengthList = List<int>.filled(50, 0);

class MangaCardContainer extends StatefulWidget {
  const MangaCardContainer({Key? key}) : super(key: key);

  @override
  State<MangaCardContainer> createState() => _MangaCardContainerState();
}

class _MangaCardContainerState extends State<MangaCardContainer> {
  ChapterList OnePieceList =
      ChapterList(MangaNames: [], LinkName: [], MangaType: 'OnePiece');
  ChapterList BorutoList =
      ChapterList(MangaNames: [], LinkName: [], MangaType: 'Boruto');
  ChapterList BokuNoHeroList =
      ChapterList(MangaNames: [], LinkName: [], MangaType: 'BokuNoHero');
  bool isLoading = true;

  var OnePieceColor = [Color(0xFF2D0505), Color(0xFFAE3535)];
  var BorutoColor = [Color(0xFF2D2605), Color(0xFF9C8D14)];
  var BokuNoHeroColor = [Color(0xFF0C2D05), Color(0xFF35AE3D)];

  @override
  void initState() {
    super.initState();

    fetchData();
  }

  fetchData() async {
    var NewOnePieceList =
        await DatabaseService().Get_ChatperList("OnePieceList");
    var NewBokuNoHeroList =
        await DatabaseService().Get_ChatperList("BokuNoHeroList");
    var NewBorutoList = await DatabaseService().Get_ChatperList("BorutoList");

    setState(() {
      OnePieceList = NewOnePieceList;
      BokuNoHeroList = NewBokuNoHeroList;
      BorutoList = NewBorutoList;
      isLoading = false;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        if (isLoading)
          const Padding(
            padding: EdgeInsets.all(18.0),
            child: Center(
              child: CircularProgressIndicator(),
            ),
          ),
        if (!isLoading)
          Wrap(
            direction: Axis.horizontal,
            spacing: 60,
            runSpacing: 40,
            children: [
              CardBody(
                mangaList: OnePieceList,
                mangaLogo: 'assets/topbar/one piece logo.png',
                mangaImage: 'assets/topbar/one piece card.png',
                mangaColor: OnePieceColor,
              ),
              CardBody(
                mangaList: BorutoList,
                mangaLogo: 'assets/topbar/boruto logo.png',
                mangaImage: 'assets/topbar/boruto card.jpg',
                mangaColor: BorutoColor,
              ),
              CardBody(
                mangaList: BokuNoHeroList,
                mangaLogo: 'assets/topbar/boku no hero logo.png',
                mangaImage: 'assets/topbar/boku no hero card.png',
                mangaColor: BokuNoHeroColor,
              ),
            ],
          ),
      ],
    );
  }
}

class CardBody extends StatefulWidget {
  final ChapterList mangaList;
  final String mangaImage;
  final String mangaLogo;
  final List<Color> mangaColor;

  const CardBody(
      {Key? key,
      required this.mangaList,
      required this.mangaImage,
      required this.mangaLogo,
      required this.mangaColor})
      : super(key: key);

  @override
  State<CardBody> createState() => _CardBodyState();
}

class _CardBodyState extends State<CardBody> {
  @override
  Widget build(BuildContext context) {
    return Stack(
      alignment: Alignment.center,
      children: [
        Container(
          height: 500,
          width: 360,
          decoration: BoxDecoration(
            borderRadius: BorderRadius.circular(10),
            color: Colors.black26,
            image: DecorationImage(
              opacity: 0.2,
              image: AssetImage(widget.mangaImage),
              fit: BoxFit.cover,
            ),
          ),
        ),
        Positioned(
            top: -20,
            child: Center(
              child: CardHead(
                mangaLogo: widget.mangaLogo,
                colors: widget.mangaColor,
              ),
            )),
        Positioned(
          top: 40,
          child: Padding(
              padding: EdgeInsets.all(20),
              child: Container(
                  height: 380,
                  width: 350,
                  child: SingleChildScrollView(
                      child: ListView.builder(
                    shrinkWrap: true,
                    itemCount: widget.mangaList.MangaNames.length,
                    itemBuilder: (context, index) {
                      return ChapterButton(
                        title: widget.mangaList.MangaNames[index],
                        url: widget.mangaList.LinkName[index],
                        mangaType: widget.mangaList.MangaType,
                        colors: widget.mangaColor,
                      );
                    },
                  )))),
        )
      ],
      clipBehavior: Clip.none,
    );
  }
}


