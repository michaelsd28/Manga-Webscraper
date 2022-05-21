import 'package:html/dom.dart' as dom;
import 'package:http/http.dart';
import 'package:mymangawebsite/services/DatabaseService.dart';

class ScrapeService {
  String OnePieceUrl = "https://mangayosh.com/reader/en/";
  String BorutoURL = "https://ww1.read-boruto.online/";
  String BokuNoURL = "https://boku-no-hero-academia.com/";

  String OnePieceScape_ImgQuery = ".fullrow .imgholder img";
  String BorutoScape_ImgQuery = ".reading-content img";
  String BokuNoScape_ImgQuery = ".entry-content img";

  Future<List<String?>> ScrapeMangaImages(String LinkName) async {
    LinkName = LinkName.replaceAll("/", "");

    String ScrapeLink = "";
    String ScrapeQuery = "";

    if (LinkName.contains("one-")||LinkName.contains("One-")) {
      print("contains one");
      ScrapeLink = OnePieceUrl + LinkName;
      ScrapeQuery = OnePieceScape_ImgQuery;
    } else if (LinkName.contains("boru")) {
      print("contains boru");
      ScrapeLink = BorutoURL + "manga/" + LinkName;
      ScrapeQuery = BorutoScape_ImgQuery;
    } else if (LinkName.contains("boku")|| LinkName.contains("my-hero") ) {
      print("contains boku");
      ScrapeLink = BokuNoURL + "manga/" + LinkName;
      ScrapeQuery = BokuNoScape_ImgQuery;
    }

    print("ScrapeLink: $ScrapeLink ScrapeQuery: $ScrapeQuery");

    final uri = Uri.parse(ScrapeLink);
    final response = await get(uri);
    dom.Document html = dom.Document.html(response.body);

    final links = html
        .querySelectorAll(ScrapeQuery)
        .map((e) => e.attributes['src'])
        .toList();

    Map<String, dynamic> ChapterMap = {
      "imgSRC": links,
      "chapterDate": DateTime.now().toString(),
      "linkName": LinkName,
    };

    if (links.length < 3) {
      //add 404 image
      links.add(
          "https://www.redeszone.net/app/uploads-redeszone.net/2021/09/Error-404-01-e1633683457508.jpg?x=480&quality=20");
      return links;
    }

    bool isChapter = false;

    DatabaseService()
        .checkIfDocumentExist(ChapterMap)
        .then((value) => isChapter = value);

    if (isChapter) {
      print("Chapter already exist");
      return links;
    } else {
      print("Chapter does not exist");
      DatabaseService().InsertDocument(ChapterMap);
      return links;
    }
  }

  Future<Map<String, dynamic>> ScrapeMangaTitles(String PageURI,
      String QuerySelect, String replaceWebsite, String MangaName) async {
    final uri = Uri.parse(PageURI);

    print("ScrapeMangaTitles: $PageURI");
    final response = await get(uri);
    dom.Document html = dom.Document.html(response.body);

    var titles = html
        .querySelectorAll(QuerySelect)
        .map((e) => e.text.replaceAll("[New]", ""))
        .toList();
    final links = html
        .querySelectorAll(QuerySelect)
        .map((e) => e.attributes['href']?.replaceAll(replaceWebsite, ""))
        .toList();

    // only first 50 chapters
    if (titles.length > 50) {
      titles.removeRange(50, titles.length);
      links.removeRange(50, links.length);
    }

    // clean up titles
    titles = titles
        .map((e) => e
            .replaceAll("								", "")
            .replaceAll("								", "")
            .replaceAll("\n", ""))
        .toList();

    Map<String, dynamic> MangaMap = {
      MangaName: {"chapterNames": titles, "linkNames": links},
      "_$MangaName": MangaName,
    };

    print("MangaMap: $MangaMap");

    return MangaMap;
  }

  String UpdateOnePieceTitles() {

    print("Updating One Piece Titles");
    String replasWebsite = "https://mangayosh.com/reader/en/";
    String QuerySelect = "tbody a";
    String OnePiecePageURI = "http://127.0.0.1:5500/One%20Piece.html";

    Map<String, dynamic> OnePieceMap = {};
    String mangaName = "OnePieceList";

    ScrapeMangaTitles(OnePiecePageURI, QuerySelect, replasWebsite, mangaName)
        .then((value) {
      OnePieceMap = value;

      print("OnePieceMap $OnePieceMap");


      if (OnePieceMap.isNotEmpty) {
        DatabaseService()
            .UpdateDocument({"_$mangaName": mangaName}, OnePieceMap);
      } else {
        print("OnePieceMap is empty");
      }
    });

    return "";
  }

  String UpdateBorutoTitles() {
    print("Updating Boruto Titles");
    String replasWebsite = "https://ww3.read-boruto.online/manga/";
    String QuerySelect = ".version-chap li a";
    String BorutoPage_URL = "https://ww1.read-boruto.online/";

    Map<String, dynamic> BorutoMap = {};
    String mangaName = "BorutoList";

    ScrapeMangaTitles(BorutoPage_URL, QuerySelect, replasWebsite, mangaName)
        .then((value) {
      BorutoMap = value;

      print("BorutoMap $BorutoMap");

      if (BorutoMap.isNotEmpty) {
        DatabaseService().UpdateDocument({"_$mangaName": mangaName}, BorutoMap);
      } else {
        print("OnePieceMap is empty");
      }
    });

    return "";
  }

  String UpdateBokuNoheroTitles() {
    print("Updating Boku No Hero Titles");
    String replasWebsite = "https://boku-no-hero-academia.com/manga/";
    String QuerySelect = ".su-posts-list-loop a";
    String BokuNoheroPage_URL = "https://boku-no-hero-academia.com/";

    Map<String, dynamic> BokuNoHeroMap = {};
    String mangaName = "BokuNoHeroList";

    ScrapeMangaTitles(BokuNoheroPage_URL, QuerySelect, replasWebsite, mangaName)
        .then((value) {
      BokuNoHeroMap = value;

      print("BorutoMap $BokuNoHeroMap");

      if (BokuNoHeroMap.isNotEmpty) {
        DatabaseService()
            .UpdateDocument({"_$mangaName": mangaName}, BokuNoHeroMap);
      } else {
        print("BokuNoHeroMap is empty");
      }
    });

    return "aaa";
  }

  void updateAllMangas() {
 UpdateOnePieceTitles();
   UpdateBorutoTitles();
    UpdateBokuNoheroTitles();
  }
}
