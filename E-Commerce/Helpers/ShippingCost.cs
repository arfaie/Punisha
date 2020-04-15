using System;
using System.Collections.Generic;

namespace ECommerce.Helpers
{
	internal class ShippingCost
	{
		private readonly Dictionary<string, int> _price500G = new Dictionary<string, int> { ["insidePart"] = 5750, ["edgePart"] = 7800, ["outsidePart"] = 8400 };

		private readonly Dictionary<string, int> _price501G1000G = new Dictionary<string, int> { ["insidePart"] = 7400, ["edgePart"] = 10000, ["outsidePart"] = 11200 };

		private readonly Dictionary<string, int> _price1001G2000G = new Dictionary<string, int> { ["insidePart"] = 9800, ["edgePart"] = 12700, ["outsidePart"] = 14000 };

		private readonly int _price2000GHigher = 2500;

		private const float Tolerance = .001f;

		private readonly float _tax = 9; // percent
		private readonly int _insurance = 800; // bimeh

		private readonly int _addPrice;

		private readonly int _sourcePartId;
		private readonly int _partId;
		private readonly float _weight;

		public ShippingCost(int sourcePartId, int partId, int weight)
		{
			_sourcePartId = sourcePartId;
			_partId = partId;
			_weight = weight;

			_addPrice = _insurance;
		}

		public float Calculate()
		{
			var checkPart = false;

			foreach (var value in postInsidePart(_sourcePartId))
			{
				if (value == _partId)
				{
					checkPart = true;
					break;
				}
			}

			float result;

			if (checkPart)
			{
				result = postPrice("edgePart") + _addPrice;
				result += result * (_tax / 100);
			}
			else if (_sourcePartId == _partId)
			{
				result = postPrice("insidePart") + _addPrice;
				result += result * (_tax / 100);
			}
			else
			{
				result = postPrice("outsidePart") + _addPrice;
				result += result * (_tax / 100);
			}
			return result;
		}

		private float postPrice(string type)
		{
			if (_weight <= 500)
			{
				return _price500G[type];
			}

			if (_weight >= 501 && _weight <= 1000)
			{
				return _price501G1000G[type];
			}

			if (_weight >= 1001 && _weight <= 2000)
			{
				return _price1001G2000G[type];
			}

			if (_weight > 2000)
			{
				var tmp = (_weight - 2000) / 1000;
				if (Math.Abs(tmp - (int)tmp) < Tolerance)
				{
					tmp++;
				}
				return _price1001G2000G[type] + tmp * _price2000GHigher;
			}

			return 0;
		}

		private List<int> postInsidePart(int sourcePartId)
		{
			List<int> result;
			switch (sourcePartId)
			{
				case 1:
					result = new List<int> { 13, 31, 11, 10, 9 };
					break;

				case 2:
					result = new List<int> { 15, 12, 8, 13 };
					break;

				case 3:
					result = new List<int> { 15, 12, 16 };
					break;

				case 4:
					result = new List<int> { 27, 20, 24, 28, 21 };
					break;

				case 5:
					result = new List<int> { 21, 28, 6, 25, 22, 23 };
					break;

				case 6:
					result = new List<int> { 5, 25, 9, 10, 11, 20, 24, 28 };
					break;

				case 7:
					result = new List<int> { 29, 9, 30, 25 };
					break;

				case 8:
					result = new List<int> { 2, 12, 17, 11, 31, 13 };
					break;

				case 9:
					result = new List<int> { 6, 25, 7, 29, 14, 13, 1, 10 };
					break;

				case 10:
					result = new List<int> { 1, 6, 9, 11 };
					break;

				case 11:
					result = new List<int> { 6, 10, 1, 31, 8, 17, 20 };
					break;

				case 12:
					result = new List<int> { 3, 16, 15, 2, 8, 17, 18 };
					break;

				case 13:
					result = new List<int> { 14, 2, 1, 31, 8, 9 };
					break;

				case 14:
					result = new List<int> { 29, 9, 13 };
					break;

				case 15:
					result = new List<int> { 3, 2, 12 };
					break;

				case 16:
					result = new List<int> { 3, 12, 18 };
					break;

				case 17:
					result = new List<int> { 18, 19, 12, 8, 11, 20 };
					break;

				case 18:
					result = new List<int> { 17, 19, 12, 16 };
					break;

				case 19:
					result = new List<int> { 18, 17, 20, 27 };
					break;

				case 20:
					result = new List<int> { 11, 17, 19, 27, 6, 24, 4 };
					break;

				case 21:
					result = new List<int> { 23, 5, 28, 4 };
					break;

				case 22:
					result = new List<int> { 26, 23, 5, 25, 30 };
					break;

				case 23:
					result = new List<int> { 26, 22, 5, 21 };
					break;

				case 24:
					result = new List<int> { 6, 28, 4, 20 };
					break;

				case 25:
					result = new List<int> { 30, 6, 5, 22, 9, 7 };
					break;

				case 26:
					result = new List<int> { 23, 22, 30 };
					break;

				case 27:
					result = new List<int> { 4, 20, 19 };
					break;

				case 28:
					result = new List<int> { 6, 24, 4, 21, 5 };
					break;

				case 29:
					result = new List<int> { 14, 9, 7 };
					break;

				case 30:
					result = new List<int> { 26, 22, 25, 7 };
					break;

				case 31:
					result = new List<int> { 13, 8, 1, 11 };
					break;

				default:
					result = new List<int>();
					break;
			}
			return result;
		}
	}

	/**

1=>تهران
2=>گیلان
3=>آذربایجان شرقی
4=>خوزستان
5=>فارس
6=>اصفهان
7=>خراسان رضوی
8=>قزوین
9=>سمنان
10=>قم
11=>مرکزی
12=>زنجان
13=>مازندران
14=>گلستان
15=>اردبیل
16=>آذربایجان غربی
17=>همدان
18=>کردستان
19=>کرمانشاه
20=>لرستان
21=>بوشهر
22=>کرمان
23=>هرمزگان
24=>چهارمحال و بختیاری
25=>یزد
26=>سیستان و بلوچستان
27=>ایلام
28=>کهگلویه و بویراحمد
29=>خراسان شمالی
30=>خراسان جنوبی
31=>البرز

--------- SQL -----------

INSERT INTO `provinces` (`id`, `name`) VALUES
(1, 'تهران'),
(2, 'گیلان'),
(3, 'آذربایجان شرقی'),
(4, 'خوزستان'),
(5, 'فارس'),
(6, 'اصفهان'),
(7, 'خراسان رضوی'),
(8, 'قزوین'),
(9, 'سمنان'),
(10, 'قم'),
(11, 'مرکزی'),
(12, 'زنجان'),
(13, 'مازندران'),
(14, 'گلستان'),
(15, 'اردبیل'),
(16, 'آذربایجان غربی'),
(17, 'همدان'),
(18, 'کردستان'),
(19, 'کرمانشاه'),
(20, 'لرستان'),
(21, 'بوشهر'),
(22, 'کرمان'),
(23, 'هرمزگان'),
(24, 'چهارمحال و بختیاری'),
(25, 'یزد'),
(26, 'سیستان و بلوچستان'),
(27, 'ایلام'),
(28, 'کهگلویه و بویراحمد'),
(29, 'خراسان شمالی'),
(30, 'خراسان جنوبی'),
(31, 'البرز');
COMMIT;

	 */
}