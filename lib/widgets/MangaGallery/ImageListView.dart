import 'dart:convert';

import 'package:flutter/material.dart';
import 'package:mymangawebsite/services/ImageService.dart';

import '../../services/DatabaseService.dart';





class ImageListView extends StatefulWidget {
  final String ChapterLink;
  final bool isBase64;

  const ImageListView({Key? key, required this.ChapterLink, required this.isBase64}) : super(key: key);

  @override
  State<ImageListView> createState() => _ImageListViewState();
}

class _ImageListViewState extends State<ImageListView> {
  List<String?> imageList = [];

  @override
  void initState() {
    super.initState();
    getImages();
  }

  void getImages() async {
    var images = await DatabaseService().Get_ImageList(widget.ChapterLink);

    ///var images64 = await ImageService().ListToBase64(widget.ChapterLink);



    images.forEach((element) {
      print(" imageList Element: *$element* ");
    });



    setState(() {
      imageList = images;
     // imageList = images64;
    });
  }

  @override
  Widget build(BuildContext context) {
    /* if (imageList.length < 2) {
      return SizedBox(
        height: ScreenSize.height,
        width: ScreenSize.width,
        child: const Text(
          "No Images",
          textAlign: TextAlign.center,
        ),
      );
    }*/

    return Column(

      children: imageList.map((element) => Image.network(element!,
        filterQuality: FilterQuality.high,
        errorBuilder: (context, error, stackTrace) {
          print("Image Error: $error");
          return Image.asset("assets/topbar/image64.png",height: 30, width: 30,);
        },

        )).toList(),

    );
  }
}