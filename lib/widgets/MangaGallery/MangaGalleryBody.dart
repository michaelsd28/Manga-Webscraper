import 'dart:ui';
import 'package:flutter/material.dart';
import 'package:mymangawebsite/model/ChapterList.dart';
import 'package:mymangawebsite/services/DatabaseService.dart';
import 'package:mymangawebsite/services/ImageService.dart';
import '../TopBar.dart';
import 'ChangePageController.dart';
import 'GalleryComboBox..dart';
import 'GoTopButton.dart';
import 'ImageListView.dart';

class MangaGalleryBody extends StatefulWidget {
  final String mangatype;
  final String chapterlink;

  const MangaGalleryBody(
      {Key? key, required this.mangatype, required this.chapterlink})
      : super(key: key);

  @override
  State<MangaGalleryBody> createState() => _MangaGalleryBodyState();
}

class _MangaGalleryBodyState extends State<MangaGalleryBody> {
  ChapterList chapterlist =
      ChapterList(MangaType: '', LinkName: [], MangaNames: []);
  String theChapterName = "";
  bool isBase64 = false;

  @override
  void initState() {
    super.initState();
    GetMangaChapterList();
  }


  @override
  Widget build(BuildContext context) {
    /// screen size
    final ScreenSize = MediaQuery.of(context).size;


    final ScrollController _scrollController = ScrollController();

    return SizedBox(
      height: ScreenSize.height,
      width: ScreenSize.width,
      child: SingleChildScrollView(
        controller: _scrollController,
        child: Column(
          children: [
            Stack(
              children: [
                Row(
                  children: [
                    Expanded(
                      child: Container(
                        height: 150,
                        child: const Scaffold(
                            appBar: PreferredSize(
                          preferredSize: Size.fromHeight(150),
                          child: MangaTopBar(),
                        )),
                      ),
                    )
                  ],
                ),
              ],
            ),
            Container(
              width: ScreenSize.width,
              decoration: BoxDecoration(
                  border: Border.all(
                    color: const Color(0xFF000000),
                    width: 5,
                    style: BorderStyle.solid,
                  ),
                  gradient: const LinearGradient(
                    begin: Alignment.topCenter,
                    end: Alignment.bottomCenter,
                    colors: [Color(0xFF11253C), Color(0xFF060C12)],
                  )),
              child: Column(
                children: [
                  GalleryComboBox(
                    currentChapterName: theChapterName,
                    chapterList: chapterlist,
                    chapterLink: widget.chapterlink,
                  ),
                  ChangePageController(
                    title: theChapterName,
                    chapterLink: widget.chapterlink,
                    chapterList: chapterlist,
                  ),
                  ConvertToBase64(
                      chapterlist: chapterlist,

                  ),
                  ImageListView(
                    ChapterLink: widget.chapterlink,
                    isBase64: isBase64,
                  ),
                  GoTopButton(
                    scrollToTopController: _scrollController,
                  ),
                ],
              ),
            ),
          ],
        ),
      ),
    );


  }

  void GetMangaChapterList() async {
    ChapterList fetchChapters =
        await DatabaseService().Get_ChatperList(widget.mangatype);

    String ChapterName = fetchChapters.LinkName[0];

    //// for each with index
    for (int i = 0; i < fetchChapters.LinkName.length; i++) {
      String cleanString =
          DatabaseService().clearSpaces(fetchChapters.LinkName[i]);

      if (cleanString == widget.chapterlink) {
        ChapterName = fetchChapters.MangaNames[i];

        break;
      }
    }

    setState(() {
      chapterlist = fetchChapters;
      theChapterName = ChapterName;
    });
  }

  void setBase64(){

    setState(() {
      isBase64=!isBase64;
    });

  }


}

class ConvertToBase64 extends StatefulWidget {

  final ChapterList chapterlist;



  const ConvertToBase64({Key? key,  required this.chapterlist}) : super(key: key);

  @override
  State<ConvertToBase64> createState() => _ConvertToBase64State();
}

class _ConvertToBase64State extends State<ConvertToBase64> {



  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: const EdgeInsets.only(bottom: 20),
      child: Container(
        alignment: Alignment.center,
        width: 180,
        height: 50,
        child: TextButton(


          style: ButtonStyle(
            overlayColor:
                MaterialStateProperty.all<Color>(Colors.blue.withOpacity(0.6)),
            backgroundColor:
                MaterialStateProperty.all<Color>(Colors.blue.withOpacity(0.2)),
            foregroundColor: MaterialStateProperty.all<Color>(Colors.white),
          ),
          onPressed: () {

            setState(() {




            });

          },
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: [
              Text("Convert to Base64"

              ),
              Image.asset(
                "assets/topbar/image64.png",
                width: 25,
                height: 25,
              )
            ],
          ),
        ),
      ),
    );
  }
}
