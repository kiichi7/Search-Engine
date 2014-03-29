using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;
/*
      '       迅龙中文分类搜索引擎 v0.7  nSearch版 
      '
      '        LGPL  许可发行
      '
      '       宁夏大学  张冬   zd4004@163.com
      ' 
      '        官网 http://blog.163.com/zd4004/
 */

namespace nSearch.ClassLibraryHTML
{
    /// <summary>
    /// 把数据变为　索引器　可以使用的数据 取数据前　５０００　字节 全角　半角转换　　去掉乱码
    /// </summary>
    class ClassTXT2IDAT
    {
        private ArrayList xxFalseT = new ArrayList();

        /// <summary>
        /// 需要替换为空格的字符
        /// </summary>
        private ArrayList xxNisoCode = new ArrayList();
        /// <summary>
        /// 乱码判别
        /// </summary>
        private string xxFalse =

        "鎮鏈夌櫥褰曟垨鑰呮偍娌℃湁鎵ц璇ユ搷浣滅殑鐩稿簲鏉冮檺銆鎮鏈夌櫥褰曟垨鑰呮偍娌℃湁鎵ц璇ユ搷浣滅殑鐩稿"
+ "簲鏉冮檺銆т俊鎭紝璇疯闂鍔ㄥ姏鎺у埗宸ョ▼鍒嗗瓙鐢熺墿瀛鍏ㄦ牎鍏叡閫変慨璇鏈绋嬩负鐢熺墿绉戝涓撲笟鐨勫熀纭"
+ "閫氳繃鏈绋嬬殑瀛佸銆佹鐗╃郴缁熷垎绫诲绛夌浉鍏冲熀纭鐭ヨ瘑锛屼负鍚庣画璇剧▼鐨勫涔犳墦涓嬭壇濂界殑鍩虹"
+ "銆鍥藉绾х簿鍝佽闈炵嚎鎬х紪杈戜笌鍒朵綔"
+ "涓轰富锛岃瑙ｉ潪绾挎ц棰戠紪缂変笌鍒朵綔鐨勫熀鏈師鐞嗗拰鏂规硶銆佽嫳鏂囧弻璇"
+ "鍒嗗瓙鐢熺墿瀛鍏ㄦ牎鍏叡閫変慨璇鏈绋嬩负鐢熺墿绉戝涓撲笟鐨勫熀纭"
+ "閫氳繃鏈绋嬬殑瀛鎺屾彙妞嶇墿褰㈡佸銆佹鐗╃郴缁熷垎绫诲绛夌浉鍏冲熀纭"
+ "鐭ヨ瘑锛屼负鍚庣画璇剧▼鐨勫涔犳墦涓嬭壇濂界殑鍩"
+ "銆鍥藉绾х簿鍝佽绋鍔ㄧ敾璁捐鍏徃鎺ㄥ嚭鐨勭煝閲忓姩鐢诲埗浣滆蒋浠躲鍗曠殑鍔ㄧ敾鍒板鏉傜殑浜や簰寮"
+ "搴旂敤绋嬪簭锛屽畠浣挎偍鍙互鍒涘缓浠讳綍浣滃搧銆傞氳繃娣诲姞鍥剧墖銆佸０闊冲拰瑙嗛锛屽彲浠ヤ娇鎮ㄧ殑"
+ "搴旂敤绋嬪簭濯掍綋涓板瘜澶氬僵銆鍥藉绾х簿鍝佽绋闈炵嚎鎬х紪杈戜笌鍒朵綔涓轰富锛岃瑙ｉ潪绾挎"
+ "ц棰戠紪缂変笌鍒朵綔鐨勫熀鏈師鐞嗗拰鏂规硶銆佽嫳鏂囧弻璇娆㈣繋浣跨敤璇剧▼锛昏緫璁捐銆"
+ "缁勬姒傝堪濈粍妗嗗寘鍚袱涓锛宸︽鍖呭惈涓鑿滃崟锛屽叾涓寘鍚绋嬪唴瀹瑰尯銆佷氦娴佸拰鍏朵粬宸ュ叿鐨勯摼鎺ャ"
+ "鍙互閰嶇疆杩欎簺閾炬帴锛屽洜姝ゅ悇闂ㄨ绋嬬殑閾炬帴鍙兘涓嶅悓銆傦級璇剧▼鍐呭妗鈥滆绋嬪唴瀹规鈥濇槸鑿滃崟閾炬帴鐨勭洰鏍囨銆傛"
+ "妗嗛粯璁や负鏃犲叆鍙璇︾粏璇存槑姝ょ粍妗嗙殑甯姪鏂囨娆㈣繋浣跨敤璇剧▼锛鍔ㄥ姏鎺у埗宸ョ▼銆"
+ "缁勬姒傝堪濈粍妗嗗寘鍚袱涓锛鑿滃崟锛屽叾涓寘鍚绋嬪唴瀹瑰尯銆佷氦娴佸拰鍏朵粬宸ュ叿鐨勯摼鎺ャ鍙互閰嶇疆杩欎簺閾炬帴锛屽洜姝ゅ悇闂ㄨ绋嬬殑閾炬"
+ "帴鍙兘涓嶅悓銆傦級璇剧▼鍐呭妗鈥滆绋嬪唴瀹规鈥濇槸鑿滃崟閾炬帴鐨勭洰鏍囨銆傛"
+ "妗嗛粯璁や负鏃犲叆鍙璇︾粏璇存槑姝ょ粍妗嗙殑甯姪鏂囨鍔ㄥ姏鎺у埗宸ョ▼銆缁勬姒傝堪鑿滃崟锛屽叾涓寘鍚绋嬪唴瀹瑰尯銆佷氦娴佸拰鍏朵"
+ "粬宸ュ叿鐨勯摼鎺ャ鍙互閰嶇疆杩欎簺閾炬帴锛屽洜姝ゅ悇闂ㄨ绋嬬殑閾炬帴鍙兘涓嶅悓銆傦級璇剧▼鍐呭妗"
 + "闄㈣嫳璇笓涓氬洓骞寸骇璇剧▼鍔ㄧ墿瀛︽槸鐢熺墿绉戝"
+ "闄㈣嫳璇笓涓氬洓骞寸骇璇剧▼璁″垝涓富骞茶绋嬩箣涓"
+ "绯荤粺璁捐濯掍綋鐞嗚涓庡疄璺姝ゅ唴瀹逛笉鍏佽璁垮璁块棶"
+ "杩滅▼鏁欒偛鐮旂┒銆绯荤粺璁捐銆姝ゅ唴瀹逛笉鍏佽璁垮璁块棶"
+ "鏁欒偛浼犳挱瀛︺濯掍綋鐞嗚涓庡疄璺鐪熸儏浼氭案鎭掑悧銆"
+ "涓汉淇℃伅鎴戠殑鎴愮哗鏁板瓧鏀跺彂绠娣诲姞娑堟伅骞冲彴閬囧埌鐨勯棶棰"
+ "鏅鸿兘鏂囨湰鍥炲娑堟伅骞冲彴閬囧埌鐨勯棶棰"
+ "鐢熸彁浜や綔涓氾紝鍒╃敤鈥滄暟瀛楁敹鍙戠鈥濅笂浼狅"
+ "紝鏂囦欢杈冨ぇ锛屼笉鑳戒笂浼狅紝璇烽棶鏂囦欢澶у皬鐨"
+ "勯檺鍒舵槸澶氬皯锛璁哄潧涓婚璇︾粏淇℃伅涓婁紶鏂囦欢鐨勫ぇ灏忛檺鍒跺湪"
+ "瀹屾垚鎿嶄綔銆傚崟鍑杈撳叆濮撴皬鐨勭涓"
+ "璧峰瀛楃杈撳叆濮撴皬鐨勭涓濯掍綋鐞嗚涓庡疄璺鎼滅储鏉′欢锛"
+ "鐢靛瓙閭欢鎼滅储鏉′欢锛"
+ "こ掠鞯奶馕陡呔倨熘模沤岱芙κ迪帜拇笱г谛率兰偷目缭绞椒⒄埂返墓ぷ鞅ǜ妫ü恕赌拇笱Ы讨肮ご蠖钜搅品巡怪"
+ "菪邪旆ā罚【俨说谖褰旖檀崛鲎殴ぷ魑被帷";


        string OtherChar = "［＼］＾＿｀｛｜｝キ゜゛ガォエェウ︰︳︴︵︶︷︸︹︺︻"
  + "︼︽︾︿﹀﹁﹂﹃﹄﹉﹊﹋﹌﹍﹎﹏﹐﹑﹒﹔﹕～￡￢￣￤￥_—"
  + "━│┃┄┅┆┇┈┉┊┋┌┍┎┏┐┑┒┓└┖┗┘┙┚┪┩"
  + "┨┧┦┥┤┣┢┡┠┟┞┞┝├┛★●◎█◆○◇▔▕▏╲╳"
  + "╱╰╯♀♂¤£¢}~{°¯¿¾½¼»º¹¸•µ´³²±Àïîëêéèçæåäãâáñòóùú"
  + "ûüýÿÐÉÇÂÃÁⅠ‰‖※℅℉≈∽√∑∏∈∮∫∴∵⊙⑶⊕⊿⌒①"
  + "②③④⑤⑥⑦⑦⑧⑨⑩⑴⑵⑷⑸⑹⑺⑻⑼⑽⑾⑿⒁⒀⒂⒄⒃⒇"
  + "⒈⒉⒊⒋⒌⒍⒐⒒⒓⒔⒕⒖⒗⒘⒚⒛─││┄┅┆┇┈┪┩┨"
  + "┺┹┷┶┴┵┳┲┱┱┰┯┮┭┬┫┻┼┽┾╀╁╂╃╄╈╉"
  + "╊╞╜╛╚╙╘╗╖═╋╮╭╫╪╩╨╧╦╤╣╢╡╠╟▋▊"
  + "▉█▇▆▄▁╳╲╱╰╯▍▎▏▓▔▕▲△▽○◇⺌⺄⺈⺁♂"
  + "♀☉★◥◤◢◎⿷⿶⺶⺮⿸」『【】〓〔〕〖〗〢〡〣〨〩ぁ"
  + "ぃぅいうおかがぎくこじすずせぜそちぢっつてなふぴび";
        /*
        private void initxxNisoCode()
        {
            xxNisoCode.Clear();

            foreach (char a in OtherChar)
            {
                if (xxNisoCode.Contains(a) == false)
                {
                    xxNisoCode.Add(a);
                }
            }
        }
        */
        public ClassTXT2IDAT()
        {
            initXXFalse();

        }

        /// <summary>
        /// 得到健康的数据
        /// </summary>
        /// <param name="dat"></param>
        /// <param name="isClearBD">是否去掉标点</param>
        /// <returns></returns>
        public String GetOneGoodData2(string dat,bool isClearBD)
        {
            if (isLUANma(dat) == true)
            {
                return "";//乱码文件请系统修正
            }

            //去掉所有的HTML数据
            string   dat1 =  GetHtml2Txt(dat);

            //数据规范在2000以内
            dat1 = dat2FiveOne(dat);

            //过滤掉不可见字符
            dat1 = stringcode(dat1);

            if (isClearBD == true)
            {
                //去掉标点
                dat1 = ClearBD(dat1);
            }

            dat1 = dat1.Replace("  ", " ");

            dat1 = dat1.Replace("　", " ");
            dat1 = dat1.Replace("   ", " ");
            dat1 = dat1.Replace("  ", " ");


            return dat1;
        }

        /// <summary>
        /// 初始化错误判别数据
        /// </summary>
        /// <param name="data"></param>
        private void initXXFalse()
        {
            xxFalseT.Clear();

            foreach (char a in xxFalse)
            {
                if (xxFalseT.Contains(a) == false)
                {
                    xxFalseT.Add(a);
                }
            }
        }

        /// <summary>
        /// 是否为乱码  1/2 含有文中的字
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool isLUANma(string data)
        {

            int si = 0;
            int ss = data.Length;

            foreach (char a in xxFalseT)
            {

                if (data.IndexOf(a) > -1)
                {
                    si = si + 1;
                }

                if (si >= ss / 2)
                {
                    return true;
                }

            }

            return false;
        }


        /// <summary>
        /// 格式化 2000
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        private string dat2FiveOne(string data)
        {
            int n1 = data.Length;

            if (n1 > 500)
            {
                data = data.Substring(0, 500);
            }

            return data;
        }


        /// <summary>
        /// 过滤掉回车等不可见的ASCII
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private  string stringcode(string data)
        {
            //乱码替换为字符
            foreach (char v in OtherChar)
            {
                data = data.Replace(v, ' ');
            }

            data = data.Replace("\0", "");

            data = data.Replace("\b", " ");
            data = data.Replace("\f", " ");
            data = data.Replace("\n", " ");
            data = data.Replace("\r", " ");
            data = data.Replace("\t", " ");
            data = data.Replace("\v", " ");

            data = data.Replace("　", " ");
            data = data.Replace("   ", " ");
            data = data.Replace("  ", " ");



            data = data.Replace("Ａ", "A");
            data = data.Replace("Ｂ", "B");
            data = data.Replace("Ｃ", "C");
            data = data.Replace("Ｄ", "D");
            data = data.Replace("Ｅ", "E");
            data = data.Replace("Ｆ", "F");
            data = data.Replace("Ｇ", "G");
            data = data.Replace("Ｈ", "H");
            data = data.Replace("Ｉ", "I");
            data = data.Replace("Ｇ", "G");
            data = data.Replace("Ｋ", "K");
            data = data.Replace("Ｌ", "L");
            data = data.Replace("Ｍ", "M");
            data = data.Replace("Ｎ", "N");
            data = data.Replace("Ｏ", "O");
            data = data.Replace("Ｐ", "P");
            data = data.Replace("Ｑ", "Q");
            data = data.Replace("Ｒ", "R");
            data = data.Replace("Ｓ", "S");
            data = data.Replace("Ｔ", "T");
            data = data.Replace("Ｕ", "U");
            data = data.Replace("Ｖ", "V");
            data = data.Replace("Ｗ", "W");
            data = data.Replace("Ｘ", "X");
            data = data.Replace("Ｙ", "Y");
            data = data.Replace("Ｚ", "Z");
            data = data.Replace("１", "1");
            data = data.Replace("２", "2");
            data = data.Replace("３", "3");
            data = data.Replace("４", "4");
            data = data.Replace("５", "5");
            data = data.Replace("６", "6");
            data = data.Replace("７", "7");
            data = data.Replace("８", "8");
            data = data.Replace("９", "9");
            data = data.Replace("０", "0");
            data = data.Replace("`", "`");
            data = data.Replace("～", "~");
            data = data.Replace("！", "!");
            data = data.Replace("·", ".");
            data = data.Replace("＃", "#");

            data = data.Replace("￥", "$");
            data = data.Replace("％", "%");
            data = data.Replace("……", " ");
            data = data.Replace("—", " ");
            data = data.Replace("＊", "*");
            data = data.Replace("（", "(");
            data = data.Replace("）", ")");
            data = data.Replace("｛", "{");
            data = data.Replace("｝", "}");
            data = data.Replace("：", ":");
            data = data.Replace("：", ";");
            data = data.Replace("？", "?");
            data = data.Replace("＋", "+");
            data = data.Replace("－", "-");
            data = data.Replace("＝", "=");
            data = data.Replace("｜", "|");
            //    data = data.Replace("", "|");

            data = data.Replace("ａ", "A");
            data = data.Replace("ｂ", "B");
            data = data.Replace("ｃ", "C");
            data = data.Replace("ｄ", "D");
            data = data.Replace("ｅ", "E");
            data = data.Replace("ｆ", "F");
            data = data.Replace("ｇ", "G");
            data = data.Replace("ｈ", "H");
            data = data.Replace("ｉ", "I");
            data = data.Replace("ｇ", "G");
            data = data.Replace("ｋ", "K");
            data = data.Replace("ｌ", "L");
            data = data.Replace("ｍ", "M");
            data = data.Replace("ｎ", "N");
            data = data.Replace("ｏ", "O");
            data = data.Replace("ｐ", "P");
            data = data.Replace("ｑ", "Q");
            data = data.Replace("ｒ", "R");
            data = data.Replace("ｓ", "S");
            data = data.Replace("ｔ", "T");
            data = data.Replace("ｕ", "U");
            data = data.Replace("ｖ", "V");
            data = data.Replace("ｗ", "W");
            data = data.Replace("ｘ", "X");
            data = data.Replace("ｙ", "Y");
            data = data.Replace("ｚ", "Z");

            data = data.ToLower();
            data = data.Trim();
            return data;

        }

        /// <summary>
        /// 去掉标点
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        private string ClearBD(string data)
        {
            data = data.Replace("/", " ");


             data = data.Replace(" 欢迎 举报 ", " ");

            data = data.Replace("swiftdog", " ");

            data = data.Replace("gamehike", " ");

           // data = data.Replace("gamehike", " ");
            
            data = data.Replace("`", " ");
            data = data.Replace("~", " ");
            data = data.Replace("!", " ");
            data = data.Replace(".", " ");
            data = data.Replace("#", " ");

            data = data.Replace("$", " ");
            data = data.Replace("%", " ");

            data = data.Replace("*", " ");
            data = data.Replace("(", " ");
            data = data.Replace(")", " ");
            data = data.Replace("{", " ");
            data = data.Replace("}", " ");
            data = data.Replace(":", " ");
            data = data.Replace(";", " ");
            data = data.Replace("?", " ");
            data = data.Replace("+", " ");
            data = data.Replace("-", " ");
            data = data.Replace("=", " ");
            data = data.Replace("|", " ");
            data = data.Replace("\\", " ");

            data = data.Replace("&ldquo", " ");
            data = data.Replace("。", " ");
            data = data.Replace("，", " ");
            data = data.Replace("：", " ");
            data = data.Replace("、", " ");
            data = data.Replace("：", " ");
            data = data.Replace("”", " ");
            data = data.Replace("“", " ");
            data = data.Replace("；", " ");
            data = data.Replace("《", " ");
            data = data.Replace("》", " ");
            data = data.Replace(",", " ");
            data = data.Replace("&rdquo", " ");
            data = data.Replace("(", " ");
            data = data.Replace(")", " ");
            data = data.Replace("？", " ");
            data = data.Replace("!", " ");
            data = data.Replace(" ！", " ");
            data = data.Replace("[", " ");
            data = data.Replace("]", " ");
            data = data.Replace(">", " ");

            data = data.Replace("【", " ");
            data = data.Replace("】", " ");

            data = data.Replace("』", " ");

            data = data.Replace("/", " ");
            data = data.Replace("<", " ");
            data = data.Replace(">", " ");

            data = data.Replace("；", " ");
            data = data.Replace("‘", " ");
            data = data.Replace("“", " ");
            data = data.Replace("”", " ");

            data = data.Replace(".", " ");
            data = data.Replace("-", " ");
            data = data.Replace("。", " ");
            data = data.Replace("|", " ");
            data = data.Replace("+", " ");
            data = data.Replace("*", " ");
            data = data.Replace("@", " ");
            data = data.Replace(":", " ");
            data = data.Replace("：", " ");
            // data = data.Replace(" ", " ");
            //  data = data.Replace(" ", " ");

            data = data.Replace("?", " ");
            data = data.Replace("λ", " ");

            data = data.Replace("[", " ");
            data = data.Replace("]", " ");
            data = data.Replace("．", " ");
            data = data.Replace(".", " ");
            data = data.Replace("↓", " ");
            data = data.Replace("?", " ");
            data = data.Replace("~", " ");
            data = data.Replace("`", " ");
            data = data.Replace("!", " ");
            data = data.Replace("@", " ");
            data = data.Replace("#", " ");
            data = data.Replace("$", " ");
            data = data.Replace("%", " ");
            data = data.Replace("^", " ");
            data = data.Replace("&", " ");
            data = data.Replace("*", " ");
            data = data.Replace("(", " ");
            data = data.Replace(")", " ");
            data = data.Replace("-", " ");
            data = data.Replace("_", " ");
            data = data.Replace("=", " ");
            data = data.Replace("+", " ");
            data = data.Replace("|", " ");
            data = data.Replace("\\", " ");
            data = data.Replace("{", " ");
            data = data.Replace("}", " ");
            data = data.Replace(":", " ");
            data = data.Replace(":", " ");
            data = data.Replace("\" ", " ");
            data = data.Replace("'", " ");
            data = data.Replace("<", " ");
            data = data.Replace(">", " ");
            data = data.Replace(",", " ");
            data = data.Replace(".", " ");
            data = data.Replace("/", " ");
            //  data = data.Replace(" ", " ");
            data = data.Replace("～", " ");
            // data = data.Replace(" ", " ");
            data = data.Replace("`", " ");
            data = data.Replace("！", " ");
            data = data.Replace("＠", " ");
            data = data.Replace("＃", " ");
            data = data.Replace("＄", " ");
            data = data.Replace("％", " ");
            data = data.Replace("︿", " ");
            data = data.Replace("＆", " ");
            data = data.Replace("＊", " ");
            data = data.Replace("（", " ");
            data = data.Replace("）", " ");
            data = data.Replace("＿", " ");
            data = data.Replace("－", " ");
            data = data.Replace("＋", " ");
            data = data.Replace("＝", " ");
            data = data.Replace("｜", " ");
            data = data.Replace("＼", " ");
            data = data.Replace("［", " ");
            data = data.Replace("］", " ");
            data = data.Replace("｛", " ");
            data = data.Replace("｝", " ");
            data = data.Replace("：", " ");
            data = data.Replace("；", " ");
            data = data.Replace("＇", " ");
            data = data.Replace("＂", " ");
            data = data.Replace("｀", " ");
            data = data.Replace("〃", " ");
            data = data.Replace("＜", " ");
            data = data.Replace("＞", " ");
            data = data.Replace("，", " ");
            data = data.Replace("．", " ");
            data = data.Replace("／", " ");
            data = data.Replace("？", " ");

            data = data.Replace("\"", " ");
            data = data.Replace("〉", " ");
            data = data.Replace("'", " ");

            data = ClearErrXowrd(data);

            return data.Trim();
        }

        /// <summary>
        /// 所有的html标记去掉 
        /// </summary>
        /// <param name="dat"></param>
        /// <returns></returns>
        private  string GetHtml2Txt(string dat)
        {

            string data = Regex.Replace(dat, "<[^>]*>", " ");

            data = data.Replace("&ldquo;", " ");
            data = data.Replace("&nbsp;", " ");
            data = data.Replace("&rdquo;", " ");

            data = data.Replace("\b", " ");
            data = data.Replace("\f", "  ");
            data = data.Replace("\n", " ");
            data = data.Replace("\r", " ");
            data = data.Replace("&nbsp;", " ");
            data = data.Replace("&gt;", " ");
            data = data.Replace("&quot;", " ");
            data = data.Replace("&nbsp;", " ");
            data = data.Replace("\t", " ");
            data = data.Replace("\v", " ");
            data = data.Replace("   ", " ");
            data = data.Replace("  ", " ");


            data = data.Replace("<br>", " ");
            data = data.Replace("&lt;", " ");
            // data = data.Replace("</a>", "");
            // data = data.Replace("</td>", "");
            data = data.Replace("<p>", " ");
            data = data.Replace("</p>", " ");
            //  data = data.Replace("</tr>", "");
            //  data = data.Replace("<tr>", "");
            //  data = data.Replace("</div>", "");
            //  data = data.Replace("<div>", "");

            data = data.Replace("<br />", " ");
            data = data.Replace("<br/>", " ");
            data = data.Replace("<dt>", " ");
            data = data.Replace("</dt>", " ");
            data = data.Replace("<dd>", " ");
            data = data.Replace("</dd>", " ");

            data = data.Replace("<--", " ");
            data = data.Replace("-->", " ");

            data = data.Replace("<-", " ");
            data = data.Replace("->", " ");

            data = data.Replace("-〉", " ");

            data = data.Replace("<%", " ");
            data = data.Replace("%>", " ");

            data = data.Replace("<ol>", " ");
            data = data.Replace("</ol>", " ");
            data = data.Replace("<li>", " ");
            data = data.Replace("</li>", " ");
            data = data.Replace("<ul>", " ");
            data = data.Replace("</ul>", " ");
            data = data.Replace("</b>", " ");
            data = data.Replace("<b>", " ");
            data = data.Replace("<i>", " ");
            data = data.Replace("</i>", " ");
            data = data.Replace("<s>", " ");
            data = data.Replace("</s>", " ");
            data = data.Replace("<nobr>", " ");
            //  data = data.Replace("</script>", " ");
            data = data.Replace("</span>", " ");
            data = data.Replace("<span>", " ");

            return data;

        }

        //
        /// <summary>
        /// 清除可能导致分词错误的数据
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        private string ClearErrXowrd(string a)
        {
            a = a.Replace("---", " ");
            a = a.Replace("===", " ");

            a = a.Replace("--", "-");
            a = a.Replace("==", "=");

            //去掉数字 和 数字空格 连续的情况

            for (int pp = 0; pp < 2010; pp++)
            {
                a = a.Replace(" "+pp.ToString()+" ", " ");
            }


            for (int pp = 0; pp < 99; pp++)
            {
                a = a.Replace(" 0" + pp.ToString() + " ", " ");
            }



            a = a.Replace("  ", " ");


            //


            return a;
        }


    }
}

/*
 
 
 
 
 
         /// <summary>
        /// 把长的数据变为短的 词典 把数据变为《KC+ZD INX=0》  
        /// </summary>
        /// <param name="HM">HTML数据列表</param>
        /// <returns></returns>
        private HTM2SHORT Long2Short(Hashtable HM)
        {
            HTM2SHORT cv = new HTM2SHORT();

            ArrayList i_i_1 = new ArrayList();
            i_i_1.Clear();
            Hashtable i_i_2 = new Hashtable();
            i_i_2.Clear();

            cv.Dict1 = i_i_1;
            cv.Dict2 = i_i_1;
            cv.HASH = i_i_2;



            ArrayList shortDict1 = new ArrayList();
            shortDict1.Clear();

            ArrayList shortDict2 = new ArrayList();
            shortDict2.Clear();

            //取标签
            foreach (System.Collections.DictionaryEntry de2 in HM)
            {

                string ShortHMONE = de2.Value.ToString();
                for (int i = 0; i < ShortHMONE.Length - 2; i++)
                {
                    int ae_1 = ShortHMONE.IndexOf('<', i);
                    int ae_2 = ShortHMONE.IndexOf('>', ae_1 + 1);
                    int ae_3 = ShortHMONE.IndexOf('<', ae_1 + 1);

                    if (ae_1 == -1 | ae_2 == -1)
                    {
                        break;
                    }
                    else
                    {
                        if (ae_3 < ae_2)
                        {
                            i = ae_1 + 1;
                        }
                        else
                        {
                            string ae_str = ShortHMONE.Substring(ae_1, ae_2 - ae_1 + 1);
                            if (ae_str.Length < 10)
                            {
                            }
                            else
                            {
                                if (shortDict1.Contains(ae_str) == false)
                                {
                                    shortDict1.Add(ae_str);  //得到一个公共的词典
                                }
                            }

                            i = ae_2;
                        }

                    }
                }
            }


            // 取标签外 
            foreach (System.Collections.DictionaryEntry de2 in HM)
            {

                string ShortHMONE = de2.Value.ToString();
                for (int i = 0; i < ShortHMONE.Length - 2; i++)
                {
                    int ae_1 = ShortHMONE.IndexOf('>', i);
                    int ae_2 = ShortHMONE.IndexOf('<', ae_1 + 1);
                    int ae_3 = ShortHMONE.IndexOf('>', ae_1 + 1);

                    if (ae_1 == -1 | ae_2 == -1)
                    {
                        break;
                    }
                    else
                    {
                        if (ae_3 < ae_2)
                        {
                            i = ae_1 + 1;
                        }
                        else
                        {
                            string ae_str = ShortHMONE.Substring(ae_1 + 1, ae_2 - ae_1 - 1);
                            if (ae_str.Length < 10)
                            {
                            }
                            else
                            {
                                if (shortDict2.Contains(ae_str) == false)
                                {
                                    shortDict2.Add(ae_str);  //得到一个公共的词典
                                }
                            }

                            i = ae_2;
                        }

                    }
                }
            }

            int short_int1 = shortDict1.Count;
            //对词典按照长度进行排序
            string[] ae_ss1 = new string[short_int1];
            for (int i = 0; i < short_int1; i++)
            {
                ae_ss1[i] = shortDict1[i].ToString();
            }

            for (int i = 0; i < short_int1; i++)
            {
                for (int j = i; j < short_int1; j++)
                {
                    if (ae_ss1[i].Length < ae_ss1[j].Length)
                    {
                        string tmp_one = ae_ss1[j];
                        ae_ss1[j] = ae_ss1[i];
                        ae_ss1[i] = tmp_one;
                    }
                }
            }
            shortDict1.Clear();


            int short_int2 = shortDict2.Count;
            //对词典按照长度进行排序
            string[] ae_ss2 = new string[short_int2];
            for (int i = 0; i < short_int2; i++)
            {
                ae_ss2[i] = shortDict2[i].ToString();
            }

            for (int i = 0; i < short_int2; i++)
            {
                for (int j = i; j < short_int2; j++)
                {
                    if (ae_ss2[i].Length < ae_ss2[j].Length)
                    {
                        string tmp_one = ae_ss2[j];
                        ae_ss2[j] = ae_ss2[i];
                        ae_ss2[i] = tmp_one;
                    }
                }
            }
            shortDict2.Clear();

            for (int i = 0; i < short_int1; i++)
            {
                shortDict1.Add(ae_ss1[i]);
            }

            for (int i = 0; i < short_int2; i++)
            {
                shortDict2.Add(ae_ss2[i]);
            }

            Hashtable new_HM = new Hashtable();
            new_HM.Clear();

            foreach (System.Collections.DictionaryEntry de2 in HM)
            {
                string a_onre = de2.Value.ToString();
                for (int u = 0; u < short_int1; u++)
                {
                    a_onre = a_onre.Replace(shortDict1[u].ToString(), "<[" + u.ToString() + ")>");
                }

                for (int u = 0; u < short_int2; u++)
                {
                    a_onre = a_onre.Replace(shortDict2[u].ToString(), "《[" + u.ToString() + ")》");
                }

                new_HM.Add(de2.Key, a_onre);
            }

            cv.Dict1 = shortDict1;
            cv.Dict2 = shortDict2;
            cv.HASH = new_HM;

            return cv;
        }

        /// <summary>
        /// 还原压缩过的单个数据
        /// </summary>
        /// <param name="dat"></param>
        /// <param name="dict"></param>
        /// <returns></returns>
        private string Short2Long(string dat, ArrayList dict1, ArrayList dict2)
        {
            for (int u = 0; u < dict1.Count; u++)
            {
                dat = dat.Replace("<[" + u.ToString() + ")>", dict1[u].ToString());
            }

            for (int u = 0; u < dict2.Count; u++)
            {
                dat = dat.Replace("《[" + u.ToString() + ")》", dict2[u].ToString());
            }

            return dat;
        }

 
 
 
 
 */