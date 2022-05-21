import 'dart:ui';

import 'package:flutter/material.dart';
import 'package:mymangawebsite/model/ChapterList.dart';
import 'package:mymangawebsite/services/DatabaseService.dart';

class ChangePageController extends StatefulWidget {
  final String title;
  final ChapterList chapterList;
  final String chapterLink;

  const ChangePageController(
      {Key? key,
      required this.title,
      required this.chapterList,
      required this.chapterLink})
      : super(key: key);

  @override
  State<ChangePageController> createState() => _ChangePageControllerState();
}

class _ChangePageControllerState extends State<ChangePageController> {
  @override
  Widget build(BuildContext context) {
    return Padding(
      padding: EdgeInsets.all(20),
      child: Container(
        // glass effect for the background
        decoration: BoxDecoration(
          //image background
          image: DecorationImage(
            image: AssetImage("assets/topbar/landing.jpg"),
            fit: BoxFit.cover,
          ),
          borderRadius: BorderRadius.circular(20),
        ),

        width: 400,
        height: 100,
        child: ClipRRect(
          borderRadius: BorderRadius.circular(20),
          child: BackdropFilter(
            filter: ImageFilter.blur(sigmaX: 10, sigmaY: 10),
            child: Card(
              color: Colors.transparent,
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(20),
              ),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceEvenly,
                children: [
                  BackPageButton(
                    title: widget.title,
                    chapterLink: widget.chapterLink,
                    chapterList: widget.chapterList,
                  ),
                  Text(
                    widget.title,
                    style: TextStyle(
                      foreground: Paint()..color = Colors.white,
                      fontSize: 18,
                    ),
                  ),
                  NextPageButton(
                      title: widget.title,
                      chapterLink: widget.chapterLink,
                      chapterList: widget.chapterList),
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}

class BackPageButton extends StatefulWidget {
  final String title;
  final ChapterList chapterList;
  final String chapterLink;

  const BackPageButton(
      {Key? key,
      required this.title,
      required this.chapterList,
      required this.chapterLink})
      : super(key: key);

  @override
  State<BackPageButton> createState() => _BackPageButtonState();
}

class _BackPageButtonState extends State<BackPageButton> {
  @override
  Widget build(BuildContext context) {
    return Container(
      child: IconButton(
        icon: Icon(
          Icons.arrow_back,
          color: Colors.white70,
        ),
        onPressed: () {
          setState(() {
            String LinkName = widget.chapterLink;
            String typeName =
                DatabaseService().CheckTypeName(widget.chapterLink);

            /// with for loop
            for (int i = 0; i < widget.chapterList.LinkName.length; i++) {
              String CompareValue =
                  widget.chapterList.LinkName[i].replaceAll(" ", "");
              if (CompareValue == LinkName &&
                  i < widget.chapterList.LinkName.length - 1) {
                LinkName =
                    widget.chapterList.LinkName[i + 1].replaceAll(" ", "");
                break;
              }
            }

            if(LinkName == widget.chapterLink){
              return;
            }

            Navigator.pushNamed(context, "/$typeName/$LinkName");


          });
        },
      ),
    );
  }
}

class NextPageButton extends StatefulWidget {
  final String title;
  final ChapterList chapterList;
  final String chapterLink;

  const NextPageButton(
      {Key? key,
      required this.title,
      required this.chapterList,
      required this.chapterLink})
      : super(key: key);

  @override
  State<NextPageButton> createState() => _NextPageButtonState();
}

class _NextPageButtonState extends State<NextPageButton> {
  @override
  Widget build(BuildContext context) {
    return Container(
      child: IconButton(
        icon: Icon(
          Icons.arrow_forward,
          color: Colors.white70,
        ),
        onPressed: () {
          setState(() {
            ///do the same thing as back page button but backwards
            /// with for loop

            String LinkName = widget.chapterLink;
            String typeName =
            DatabaseService().CheckTypeName(widget.chapterLink);

            /// with for loop
            for (int i = 0; i < widget.chapterList.LinkName.length; i++) {
              String CompareValue =
              widget.chapterList.LinkName[i].replaceAll(" ", "");
              if (CompareValue == LinkName && i > 0) {
                LinkName =
                    widget.chapterList.LinkName[i - 1].replaceAll(" ", "");
                break;
              }
            }

            if(LinkName == widget.chapterLink){
             return;
            }



            Navigator.pushNamed(context, "/$typeName/$LinkName");



          });
        },
      ),
    );
  }
}
