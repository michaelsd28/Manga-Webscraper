//import 'dart:html' as html;

import 'dart:convert';
import 'dart:typed_data';

import 'package:flutter/material.dart';
import 'package:http/http.dart';
import 'package:http_parser/src/media_type.dart';
import 'package:http/http.dart' as http;

class DriveService {
  final String _token = 'ya29.a0ARrdaM9at75nChZ03fFO_65Cet2KeGSuT-RZAFlCAvaNoSxaHM4AJyNV2ZEOhKk8mpt5i-BUe6e48XvIRVnUnlgl3PnNJ0IXLCo-d31__cSg5pQaO4a9tAeTQ0-ax2RRKYP4jQA0fofgesVtgAGy7ERyzsNY';

  /// http upload file

  Future<String?> uploadFile() async {
    var headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer $_token',
    };
    var request = http.MultipartRequest(
        'POST', Uri.parse('https://www.googleapis.com/upload/drive/v3/files'));


    http.MultipartFile requestJson = http.MultipartFile.fromString(
        'metadata', '{"name": "test.txt", "text/plain"}',
        contentType: MediaType('application', 'json'),
        filename: 'metadata.json');



    /// write hello world to file
    String helloWorld = 'Hello World!';
    List<int> helloWorldBytes = utf8.encode(helloWorld);

    request.files.add(requestJson);
    request.headers.addAll(headers);
    request.files.add(http.MultipartFile.fromBytes('file', helloWorldBytes,
        filename: 'test.txt', contentType: MediaType('text', 'plain')));





    print("request.files.length: ${request.files.length} request.files[1].length: ${request.files[1].length} request.files[1].filename: ${request.files[1].filename} request.files[1].contentType: ${request.files[1].contentType} ");




    request.headers.addAll(headers);

    http.StreamedResponse response = await request.send();

    if (response.statusCode == 200) {
      print("Success");
      print(await response.stream.bytesToString());
    } else {
      print("Error");
      print(
          "response.reasonPhrase: ${response.reasonPhrase} response.statusCode: ${response.statusCode} response.body: ${await response.stream.bytesToString()} response.headers: ${response.headers}");
    }

    return "";
  }

  /// convert image url to streambytes
  void ConvertImageUrlToStreamBytes(String url) async {
    var headers = {
      'Content-Type': 'application/json; charset=UTF-8',
      'Authorization': 'Bearer $_token',
    };
  }

  Uint8List encode(String s) {
    var encodedString = utf8.encode(s);
    var encodedLength = encodedString.length;
    var data = ByteData(encodedLength + 4);
    data.setUint32(0, encodedLength, Endian.big);
    var bytes = data.buffer.asUint8List();
    bytes.setRange(4, encodedLength + 4, encodedString);
    return bytes;
  }

  /// image url to streambytes

}
