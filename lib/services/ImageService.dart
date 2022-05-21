import 'dart:convert';
import 'dart:io';
import 'dart:ui';

import 'package:dio/dio.dart';
import 'package:http/http.dart' as http;
import 'package:mymangawebsite/services/DatabaseService.dart';




class ImageService   {



  // download image to assets

  Future<String?> networkImageToBase64(String imageUrl) async {
    http.Response response = await http.get(Uri.parse(imageUrl) );
    final bytes = response.bodyBytes;
    // ignore: unnecessary_null_comparison
    return (bytes != null ? base64Encode(bytes) : null);
  }


  Future<List<String?>> ListToBase64(String LinkName) async {

  var imagesUrl = await  DatabaseService().Get_ImageList(LinkName);
    List<String?> base64List = [];
int count = 0;
    for (var imageUrl in imagesUrl)  {
      String? base64 = await networkImageToBase64(imageUrl!);

      print("base64 $base64");

      base64List.add(base64!);
      count++;
    }

    return base64List;
  }





  }






