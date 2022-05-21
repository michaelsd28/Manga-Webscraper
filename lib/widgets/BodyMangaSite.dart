import 'dart:io';

import 'package:flutter/material.dart';
import 'package:mymangawebsite/services/DatabaseService.dart';
import 'package:mymangawebsite/services/ScrapeService..dart';
import 'package:mymangawebsite/widgets/BodyLibrary/MangaCardContainer.dart';

import 'package:mymangawebsite/widgets/MangaGallery/MangaGalleryBody.dart';

class BodyMangaSite extends StatelessWidget {
  const BodyMangaSite({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    final _ScreenSize = MediaQuery.of(context).size;
    return Container(
      width: _ScreenSize.width,
      height: _ScreenSize.height - 170,
      decoration: BoxDecoration(
          borderRadius: const BorderRadius.only(
              topLeft: Radius.circular(15), topRight: Radius.circular(15)),
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
      alignment: Alignment.topCenter,
      child: SingleChildScrollView(

        child: Container(
          width: _ScreenSize.width - 10,
          child: Padding(
            padding: EdgeInsets.all(10),
            child: Column(
              children: const [TopMangaRefresh(), MangaCardContainer()],
            ),
          ),
        ),
      ),
    );
  }
}

class TopMangaRefresh extends StatefulWidget {
  const TopMangaRefresh({Key? key}) : super(key: key);

  @override
  State<TopMangaRefresh> createState() => _TopMangaRefreshState();
}

class _TopMangaRefreshState extends State<TopMangaRefresh> {
  bool isHovered = false;

  @override
  Widget build(BuildContext context) {
    final hovered = Matrix4.identity()..rotateZ(180);
    final transform = isHovered ? hovered : Matrix4.identity();

    return Padding(
      padding: EdgeInsets.all(20),
      child: Column(children: [
        Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            DefaultTextStyle(
              style: TextStyle(
                  color: Colors.white, fontSize: 20, letterSpacing: 1),
              child: Text(
                "Mangas",
                textAlign: TextAlign.center,
                style: TextStyle(
                    foreground: Paint()..color = Colors.white,
                    fontSize: 20,
                    letterSpacing: 1),
              ),
            ),
            MouseRegion(
              onHover: (event) => setState(() {
                isHovered = true;
              }),
              onExit: (event) => setState(() {
                isHovered = false;
              }),
              child: Padding(
                  padding: EdgeInsets.only(left: 10),
                  child: AnimatedContainer(
                    transformAlignment: Alignment.center,
                    duration: Duration(milliseconds: 500),
                    alignment: Alignment.center,
                    transform: transform,
                    child: Card(
                        clipBehavior: Clip.hardEdge,
                        color: Colors.transparent,
                        shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(20)),
                        child: IconButton(
                          onPressed: ()  {

                            ScrapeService().updateAllMangas();
                              Navigator.pop(context,"/");
                              Navigator.pushNamed(context, "/");



                          },
                          icon: Image.asset("assets/topbar/refresh mangas.png"),
                        )),
                  )),
            ),
          ],
        ),
        Padding(
          padding: EdgeInsets.only(top: 0),
          child: Divider(
            color: Colors.black26,
          ),
        ),
      ]),
    );
  }
}
