# ChinesePopulationPrediction
CreateAModelToCreateChinesePopulation

1.此程序是本人在阅读知乎文章突发奇想后，想要编写一个模拟程序的。

我是从知乎上，了解这个问题的。
https://www.zhihu.com/question/337024044/answer/898498867

想要解决一个问题，最好的方法是能建立一套模型，去模拟打工仔的行为、企业的行为、政府的行为。

 ![picture](https://raw.githubusercontent.com/sxtgyrq/ChinesePopulationPrediction/master/img/996.png)


 ┌┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┬┐
 ├┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┼┤
 └┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┴┘

如上图所示，坐标轴，横坐标0-100，0代表的政府是高福利型的社会，参照北欧、法国等国家，100代表的是资本性国家，参照中国、美国。纵坐标0%-100%，代表着谈恋爱一年内的成功率。
如图中的蓝色曲线就是政府行为对平均恋爱成功率的影响。灰色曲线是某个企业的老板在同样情况下，还鼓吹996对恋爱成功率的影响。

我们定义横坐标为政府的资本性，反之定义为政府的福利性。

政府的资本性越高，教育孩子所花费的成本也越高。
政府的资本性越高，生育孩子所花费的成本也越高。
暂时假设这种关系成正比线性关系。

当政府的福利性非常好时，生一胎、二胎、三胎、四胎的成本是一样的。即教育和医疗几乎免费。

政府的资本性高于一定值时，企业鼓吹996是福报获得的收益也就越高。
政府的福利性高于一定值时，企业鼓吹996是福报会损失收益。