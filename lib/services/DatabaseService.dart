import 'dart:convert';
import 'dart:io' show Platform;
import 'package:http/http.dart' as http;
import 'package:html/dom.dart' as dom;
import 'package:http/http.dart';
import 'package:mymangawebsite/services/ScrapeService..dart';


import '../model/ChapterList.dart';


class DatabaseService {
  String mongoUrl =
      'mongodb+srv://michaelsd28:mypassword28@cluster0.cneai.mongodb.net/boku_no_hero_mangaDB?authSource=admin&replicaSet=atlas-x7tzqc-shard-0&readPreference=primary&ssl=true';

  String localUrl = 'mongodb://localhost:27017/testdb';




  void InsertDocument(Map<String, dynamic>? newDocument) async {

    bool ifDocument = await checkIfDocumentExist(newDocument!);

    if(ifDocument == true){
      print('Document already exist');
      return ;
    }


    final uri = Uri.parse(
        'https://data.mongodb-api.com/app/data-ocnfu/endpoint/data/beta/action/insertOne');
    final headers = {
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods':
          'POST, GET, OPTIONS, PUT, DELETE, PATCH, HEAD',
      'Access-Control-Headers':
          'Origin, X-Requested-With, Content-Type, Accept',
      'api-key':
          'Qq2LNwt55HfXc29Zg3WSTBFfc2pOg8Lqdh8SVLso9edC0TZBebulnE0b7IZWlne2'
    };
    Map<String, dynamic> body = {
      "dataSource": "Cluster0",
      "database": "boku_no_hero_mangaDB",
      "collection": "chapters",
      "document": newDocument
    };
    String jsonBody = json.encode(body);
    final encoding = Encoding.getByName('utf-8');

    Response response = await post(
      uri,
      headers: headers,
      body: jsonBody,
      encoding: encoding,
    );

    int statusCode = response.statusCode;
    String responseBody = response.body;

    if (statusCode < 200 || statusCode > 400) {
      print("Error while fetching data");
      throw new Exception("Error while fetching data");
    } else {
      print(
          "successfully inserted: " +
          responseBody +
          " status code: " +
          statusCode.toString()
            );
    }
  }


  void UpdateDocument(Map <String, dynamic> FilterDoc ,  Map<String, dynamic>  newDoc ) async {
    bool ifDocument = await checkIfDocumentExist(FilterDoc);

    if (ifDocument == false) {

      print('Document does not exist : ' + FilterDoc.toString());
      return;
    }


    final uri = Uri.parse(
        'https://data.mongodb-api.com/app/data-ocnfu/endpoint/data/beta/action/updateOne');
    final headers = {
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods':
          'POST, GET, OPTIONS, PUT, DELETE, PATCH, HEAD',
      'Access-Control-Headers':
          'Origin, X-Requested-With, Content-Type, Accept',
      'api-key':
          'Qq2LNwt55HfXc29Zg3WSTBFfc2pOg8Lqdh8SVLso9edC0TZBebulnE0b7IZWlne2'
    };
    Map<String, dynamic> body = {
      "dataSource": "Cluster0",
      "database": "boku_no_hero_mangaDB",
      "collection": "chapters",
      "filter": FilterDoc,
      "update":   newDoc ,
    };
    String jsonBody = json.encode(body);
    final encoding = Encoding.getByName('utf-8');

    Response response = await post(
      uri,
      headers: headers,
      body: jsonBody,
      encoding: encoding,
    );

    int statusCode = response.statusCode;
    String responseBody = response.body;

    if (statusCode < 200 || statusCode > 400) {
      print("Error while fetching data");
      throw new Exception("Error while fetching data");
    } else {
      print(
          "successfully updated: " +
          responseBody +
          " status code: " +
          statusCode.toString()
            );
    }


    }


  String CheckTypeName(String typeName) {
    if (typeName.contains('-') && typeName.contains('one')) {
      return "OnePieceList";
    } else if (typeName.contains('-') && typeName.contains('boruto')) {
      return "BorutoList";
    } else if (typeName.contains('-') && typeName.contains('boku')) {
      return "BokuNoHeroList";
    } else {
      return "";
    }
  }

  String ClearChapterList(String stringName) {
    String newString = stringName
        .replaceAll('\\"', '"')
        .replaceAll('"{"', '{"')
        .replaceAll('/', '')
        .replaceAll('}"', '}')
        .replaceAll('Naruto Next Generations,', '')
        .replaceAll('academia,', 'academia')
        .replaceAll('Academia,', 'Academia');

    return newString;
  }

  Future<ChapterList> Get_ChatperList(String TypeName) async {
    var ifError = ChapterList(
      MangaNames: [],
      LinkName: [],
      MangaType: TypeName,
    );

    // TypeName = CheckTypeName(TypeName);

    var MangaList =
        ChapterList(MangaNames: [], LinkName: [], MangaType: TypeName);

    final uri = Uri.parse(
        'https://data.mongodb-api.com/app/data-ocnfu/endpoint/data/beta/action/findOne');
    final headers = {
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods':
          'POST, GET, OPTIONS, PUT, DELETE, PATCH, HEAD',
      'Access-Control-Headers':
          'Origin, X-Requested-With, Content-Type, Accept',
      'api-key':
          'Qq2LNwt55HfXc29Zg3WSTBFfc2pOg8Lqdh8SVLso9edC0TZBebulnE0b7IZWlne2'
    };

    Map<String, dynamic> body = {
      "dataSource": "Cluster0",
      "database": "boku_no_hero_mangaDB",
      "collection": "chapters",
      "filter": {'_$TypeName': '$TypeName'}
    };

    String jsonBody = json.encode(body);

    final encoding = Encoding.getByName('utf-8');

    Response response = await post(
      uri,
      headers: headers,
      body: jsonBody,
      encoding: encoding,
    );

    int statusCode = response.statusCode;
    String responseBody = response.body;

    String cleanJson = ClearChapterList(responseBody);

    var jsonResponse = json.decode(cleanJson);






    var MangaNames = jsonResponse['document'][TypeName]['chapterNames'];
    var LinkNames = jsonResponse['document'][TypeName]['linkNames'];

    String stringNames = json.encode(MangaNames);
    String stringLinks = json.encode(LinkNames);

    List<String> TitlesArray = MangaNames.toString().split(",");
    List<String> LinksArray = LinkNames.toString().split(",");

    ///clean the array from [ and ]
    TitlesArray = TitlesArray.map((String s) => s
        .replaceAll('[', '')
        .replaceAll('[', '')
        .replaceAll("Chapter", "")
        .replaceAll("Boku No Hero ", "My hero ")).toList();
    LinksArray =
        LinksArray.map((String s) => s.replaceAll('[', '').replaceAll('[', ''))
            .toList();

    MangaList.MangaNames = TitlesArray;
    MangaList.LinkName = LinksArray;

    print(
        "${TitlesArray.length} ${LinksArray.length} status code: $statusCode");

    if (statusCode < 200 || statusCode > 400) {
      print("Error while fetching data");

      return ifError;
    } else {
      return MangaList;
    }

    // print(+ "\n" + " status:  " + statusCode.toString());
  }

  String clearSpaces(String myString) {
    String newString = '';

    newString = myString.replaceAll(' ', '');

    if (newString[0] == ' ') {
      newString = newString.replaceFirst(' ', '');
    } else if (newString[newString.length - 1] == ' ') {
      newString =
          newString.replaceRange(newString.length - 1, newString.length, '');
    }
    return newString;
  }

  Future<List<String?>> Get_ImageList(String LinkName) async {
    var ifError = List<String>.empty();
    List<String?> ImageList = List<String>.empty();

    LinkName = clearSpaces(LinkName);

    // clear all spaces from the link
    LinkName = LinkName.replaceAll(' ', '');

    final uri = Uri.parse(
        'https://data.mongodb-api.com/app/data-ocnfu/endpoint/data/beta/action/findOne');
    final headers = {
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods':
          'POST, GET, OPTIONS, PUT, DELETE, PATCH, HEAD',
      'Access-Control-Headers':
          'Origin, X-Requested-With, Content-Type, Accept',
      'api-key':
          'Qq2LNwt55HfXc29Zg3WSTBFfc2pOg8Lqdh8SVLso9edC0TZBebulnE0b7IZWlne2'
    };

    Map<String, dynamic> body = {
      "dataSource": "Cluster0",
      "database": "boku_no_hero_mangaDB",
      "collection": "chapters",
      "filter": {"linkName": LinkName}
    };

    String jsonBody = json.encode(body);

    final encoding = Encoding.getByName('utf-8');

    Response response = await post(
      uri,
      headers: headers,
      body: jsonBody,
      encoding: encoding,
    );

    int statusCode = response.statusCode;
    String responseBody = response.body;

    if (json.decode(responseBody)['document'] == null) {

      ImageList = await ScrapeService().ScrapeMangaImages(LinkName);

      return ImageList;

    }

    if (statusCode == 200) {
      var jsonResponse = json.decode(responseBody)['document']['imgSRC'];
      String stringLinks = json.encode(jsonResponse);

      ImageList = ConvertStringArray(stringLinks);
      /*
      ImageList.forEach((element) {
        print(element + "\n" + "status: $statusCode");
      });*/

      return ImageList;
    } else {
      print("Error while fetching data");
      return ifError;
    }
  }

  List<String> ConvertStringArray(String Array) {
    if (Array[0] == '[') {
      Array = Array.substring(1, Array.length);
    }
    if (Array[Array.length - 1] == ']') {
      Array = Array.substring(0, Array.length - 1);
    }

    List<String> ArrayList = Array.split(",");

    /// clean all taps from the array
    ArrayList = ArrayList.map((String s) =>
            s.replaceAll('\\t', '').replaceAll('\\n', '').replaceAll(" ", ""))
        .toList();

    /// CLEAN QUOTES
    ArrayList = ArrayList.map((String s) => s.replaceAll('"', '')).toList();

    if (ArrayList.length < 3) {}

    return ArrayList;
  }


  Future<bool> checkIfDocumentExist(Map<String,dynamic> document) async {

    final uri = Uri.parse(
        'https://data.mongodb-api.com/app/data-ocnfu/endpoint/data/beta/action/findOne');
    final headers = {
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods':
      'POST, GET, OPTIONS, PUT, DELETE, PATCH, HEAD',
      'Access-Control-Headers':
      'Origin, X-Requested-With, Content-Type, Accept',
      'api-key':
      'Qq2LNwt55HfXc29Zg3WSTBFfc2pOg8Lqdh8SVLso9edC0TZBebulnE0b7IZWlne2'
    };

    Map<String, dynamic> body = {
      "dataSource": "Cluster0",
      "database": "boku_no_hero_mangaDB",
      "collection": "chapters",
      "filter": document
    };

    String jsonBody = json.encode(body);

    final encoding = Encoding.getByName('utf-8');

    Response response = await post(
      uri,
      headers: headers,
      body: jsonBody,
      encoding: encoding,
    );


    int statusCode = response.statusCode;
    String responseBody = response.body;

    if (json.decode(responseBody)['document'] == null) {
      return false;
     } else {
      return true;
    }


  }

  void deleteFromDB(Map <String,dynamic> filter) async{
    final uri = Uri.parse(
        'https://data.mongodb-api.com/app/data-ocnfu/endpoint/data/beta/action/deleteOne');
    final headers = {
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods':
      'POST, GET, OPTIONS, PUT, DELETE, PATCH, HEAD',
      'Access-Control-Headers':
      'Origin, X-Requested-With, Content-Type, Accept',
      'api-key':
      'Qq2LNwt55HfXc29Zg3WSTBFfc2pOg8Lqdh8SVLso9edC0TZBebulnE0b7IZWlne2'
    };

    Map<String, dynamic> body = {
      "dataSource": "Cluster0",
      "database": "boku_no_hero_mangaDB",
      "collection": "chapters",
      "filter": filter
    };

    String jsonBody = json.encode(body);

    final encoding = Encoding.getByName('utf-8');

    Response response = await post(
      uri,
      headers: headers,
      body: jsonBody,
      encoding: encoding,
    );

    int statusCode = response.statusCode;
    String responseBody = response.body;

    if (statusCode == 200) {

      print("Deleted: " + responseBody + " from DB" + filter.toString());

    } else {
      print("Error while fetching data");
    }
  }




  void refetchImageList(String linkName) {

    Map <String,dynamic> filter = {
      "linkName": linkName
    };


    deleteFromDB(filter);

    ScrapeService().ScrapeMangaImages(linkName);



  }
}
