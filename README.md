# Csv for Unity

CSV 데이터를 분석하여 레코드 단위의 문자열 목록으로 변환하는 유니티용 라이브러리입니다.

## 시작하기

### 문자열로부터 읽기

`CsvReader.Create` 메서드를 사용하여 `ICsvReader<int>` 개체를 생성하고 `Read` 메서드를 호출하여 레코드 단위로 열거할 수 있습니다.

```csharp
using UnityEngine;
using Macaron.Csv;

var str =
    "Accuracy International,AWP,.243 Winchester\r\n" +
    "Walther,WA 2000,7.62x51mm NATO\r\n";

using (var reader = CsvReader.Create(str, CsvReaderSettings.Default))
{
    // Read 메서드가 true를 반환하면 Record 속성에 값이 할당됩니다.
    while (reader.Read())
    {
        // Record는 IList<string> 인터페이스를 구현하지만 값을 변경하는 것은 허용하지 않습니다.
        var record = reader.Record;
        Debug.LogFormat("{0} {1} {2}", record[0], record[1], record[2]);

        // 또는 Get 메서드를 사용하여 필드에 접근할 수 있습니다.
        // Debug.LogFormat("{0} {1} {2}", record.Get(0), record.Get(1), record.Get(2));
    }
}

// 결과
// Accuracy International AWP .243 Winchester
// Walther WA 2000 7.62x51mm NATO
```

### `Stream`이나 `TextReader`로부터 읽기

문자열이 아닌 스트림으로부터 `ICsvReader<int>` 개체를 생성할 수 있습니다.

```csharp
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Macaron.Csv;

var str =
    "Heckler & Koch,PSG1,76x51mm NATO\r\n" +
    "Denel,NTW-20,20x82mm\r\n";
var stream = new MemoryStream(Encoding.UTF8.GetBytes(str));

// 또는 TextReader를 사용할 수 있습니다.
// var textReader = new StreamReader(stream, Encoding.UTF8);
// using (var reader = CsvReader.Create(textReader, CsvReaderSettings.Default))

using (var reader = CsvReader.Create(stream, CsvReaderSettings.Default))
{
    while (reader.Read())
    {
        Debug.Log(string.Join(" ", reader.Record.ToArray()));
    }
}

// 결과
// Heckler & Koch PSG1 76x51mm NATO
// Denel NTW-20 20x82mm
```

### 헤더를 사용하여 레코드의 특정 필드에 접근

`ICsvHeaderPolicy<string>`을 구현한 개체를 제공하여 헤더를 생성하는 방식을 지정하면 인덱스가 아닌 문자열을 사용하여 특정 열의 필드에 접근할 수 있습니다.

```csharp
using UnityEngine;
using Macaron.Csv;

var str =
    "Manufacturer,Name,Cartridge\r\n" +
    "Accuracy International,AWP,.243 Winchester\r\n" +
    "Walther,WA 2000,7.62x51mm NATO\r\n";

// 첫 번째 레코드를 헤더로 사용합니다.
var headerPolicy = CsvHeaderPolicies.FirstRecord();

// CSV 데이터가 헤더를 가지지 않는 경우 직접 입력할 수 있습니다.
// var headerPolicy = CsvHeaderPolicies.UserDefined(new[] { "Manufacturer", "Name", "Cartridge" });

using (var reader = CsvReader.Create(str, CsvReaderSettings.Default, headerPolicy))
{
    while (reader.Read())
    {
        var record = reader.Record;
        Debug.LogFormat(
            "{0} {1} {2}",
            record.Get("Manufacturer"),
            record.Get("Name"),
            record.Get("Cartridge"));
    }
}

// 결과
// Accuracy International AWP .243 Winchester
// Walther WA 2000 7.62x51mm NATO
```

### `CsvReaderSettings`를 사용하여 CSV 형식을 지정

CSV 형식과 관련된 설정을 지정하여 `ICsvReader<T>` 개체를 생성할 수 있습니다.

값 | 형식 | 설명
-- | -- | --
FieldSeparator | char | 필드 구분자. `\r`, `\n`일 수 없습니다.
Quote | char? | 인용부호. `FieldSeparator`, `\r`, `\n`일 수 없습니다. `null`인 경우 인용부호에 대한 처리를 하지 않습니다.
Escape | char? | 이스케이프. `FieldSeparator`, `\r`, `\n`일 수 없습니다. `null`인 경우 이프케이프에 대한 처리를 하지 않으며, CSV 데이터가 이스케이프를 포함하고 있는 경우 올바르게 분석하지 못할 수 있습니다.
RecordTerminator | CsvRecordTerminator? | 레코드 구분자. `null`인 경우 첫 번째 레코드의 구분자를 사용합니다. `Quote`와 `RecordTerminator`가 모두 `null`인 경우 필드가 새줄 문자를 포함하고 있다면 올바르게 분석하지 못합니다.
TrimMode | CsvTrimMode | 인용되지 않은 필드의 전후 공백을 처리하는 방법.
NullValue | string | `null`이 아니라면 이 값과 동일한 값을 가지는 필드는 해당 값 대신 `null` 값을 가지게 됩니다. 값 비교는 `TrimMode`가 적용된 후에 이루어지며 서수 비교를 수행합니다.

`CsvReaderSettings`는 자주 사용하는 형식에 대해 미리 정의된 정적 필드를 가지고 있습니다.

필드 | 설명
-- | --
Default | 기본값. RFC4180 형식을 따르나 `RecordTerminator` 값이 `null`입니다.
RFC4180 | RFC4180 형식. 마이크로소프트 엑셀, 구글 스프레드시트 등 일반적인 스프레드시트 프로그램에서 CSV 형식으로 내보낸 경우 사용할 수 있습니다.
MySQL | `SELECT ... INTO OUTFILE` 쿼리의 기본 옵션으로 생성된 CSV에 대한 형식.

`CsvReaderSettings`는 구조체 형식이기 때문에 기존값을 복사 후 수정할 수 있습니다.

```csharp
var settings = CsvReaderSettings.Default;
settings.FieldSeparator = '\t'; // 기존값에 영향을 주지 않습니다.
```

### `ICsvRecord<T>.Parse` 확장 메서드

`ICsvReader<T>.Record` 속성의 형식인 `ICsvRecord<T>`는 필드값을 변환하는 일반적인 방법으로 `Parse` 확장 메서드를 가지고 있습니다. `Parse` 호출 후 이어지는 메서드 호출을 통해 값을 변환할 수 있습니다.

```csharp
using System;
using UnityEngine;
using Macaron.Csv;

[Flags]
enum Actions
{
    Unknown = 0x00,
    GasOperated = 0x01,
    RotatingBolt = 0x02,
    BoltAction = 0x04
}

var str =
    "No,Name,Mass,Action\r\n" +
    "1,AWP,6.5,BoltAction\r\n" +
    "2,WA 2000,6.95,\"GasOperated, RotatingBolt\"\r\n";
var headerPolicy = CsvHeaderPolicies.FirstRecord();

using (var reader = CsvReader.Create(str, CsvReaderSettings.Default, headerPolicy))
{
    while (reader.Read())
    {
        var record = reader.Record;

        // 값 형식은 nullable 형식이며 필드값이 빈 문자열이거나 null이라면 null을 반환합니다.
        int? number = record.Parse("No").AsInt32();
        // 기본값을 지정하려면 ?? 연산자를 사용합니다.
        // int number = record.Parse("No").AsInt32() ?? 0;

        string name = record.Get("Name") ?? string.Empty;
        // 문자열은 레코드의 Get 메서드를 사용하는 것이 기본이지만 빈 문자열과 null에 대해 일반화된
        // 처리를 위해 AsString 메서드를 가지고 있습니다.
        // string name = record.Parse("Name").AsString() ?? string.Empty;

        float mass = record.Parse("Mass").AsSingle() ?? 0.0f;
        Actions action = record.Parse("Action").AsEnum<Actions>() ?? Actions.Unknown;

        Debug.LogFormat("No: {0}, Name: {1}, Mass: {2}kg, Action: {3}", number, name, mass, action);
    }
}

// 결과
// No: 1, Name: AWP, Mass: 6.5kg, Action: BoltAction
// No: 2, Name: WA 2000, Mass: 6.95kg, Action: GasOperated, RotatingBolt
```

`Boolean`, `Byte`, `Char`, `Decimal`, `Double`, `Int16`, `Int32`, `Int64`, `SByte`, `Single`, `UInt16`, `UInt32`, `UInt64`, `Enum`, `DateTime`, `TimeSpan`, `DateTimeOffset`, `Guid`, `String`, `Uri`와 유니티의 `Color`, `Color32` 형식에 대한 변환 메서드가 정의되어 있습니다.

#### 단일 필드를 분할하여 필드 배열을 생성

`Parse` 메서드를 호출한 후, 변환 메서드가 아닌 `Split` 메서드를 호출하여 필드값을 지정한 구분자로 분할한 필드 배열을 생성할 수 있습니다.

```csharp
using System;
using System.Linq;
using UnityEngine;
using Macaron.Csv;

var str =
    "Name,Cartridge\r\n" +
    "AWP,\"7.62x51mm NATO,.308 Winchester,.243 Winchester\"\r\n" +
    "WA 2000,\"7.62x51mm NATO,.300 Winchester Magnum,7.5x55mm Swiss\"\r\n";
var headerPolicy = CsvHeaderPolicies.FirstRecord();

using (var reader = CsvReader.Create(str, CsvReaderSettings.Default, headerPolicy))
{
    Func<ICsvRecordExtensionMethod.Field, string> enclose = f => '"' + f.Value + '"';

    while (reader.Read())
    {
        var record = reader.Record;
        var name = record.Get("Name");

        // 구분자는 문자, 문자열, 정규표현식을 사용할 수 있습니다.
        var cartridges = record.Parse("Cartridge").Split(',').Select(enclose).ToArray();

        Debug.LogFormat("{0}: {1}", name, string.Join(", ", cartridges));
    }
}

// 결과
// AWP: "7.62x51mm NATO", ".308 Winchester", ".243 Winchester"
// WA 2000: "7.62x51mm NATO", ".300 Winchester Magnum", "7.5x55mm Swiss"
```

#### 여러 필드를 합쳐 필드 배열을 생성

여러 개의 열 이름을 인자로 받는 `Parse` 메서드를 호출하여 여러 필드를 합쳐서 필드 배열을 작성할 수 있습니다.

```csharp
using System;
using System.Linq;
using UnityEngine;
using Macaron.Csv;

var str =
    "Name,Cartridge1,Cartridge2,Cartridge3\r\n" +
    "AWP,7.62x51mm NATO,.308 Winchester,.243 Winchester\r\n" +
    "PSG1,7.62x51mm NATO,,\r\n";
var headerPolicy = CsvHeaderPolicies.FirstRecord();

using (var reader = CsvReader.Create(str, CsvReaderSettings.Default, headerPolicy))
{
    Func<ICsvRecordExtensionMethod.Field, bool> hasValue = f => !string.IsNullOrEmpty(f.Value);
    Func<ICsvRecordExtensionMethod.Field, string> enclose = f => '"' + f.Value + '"';

    while (reader.Read())
    {
        var record = reader.Record;
        var name = record.Get("Name");
        var cartridges = record
            .Parse("Cartridge1", "Cartridge2", "Cartridge3")
            .Where(hasValue)
            .Select(enclose)
            .ToArray();

        Debug.LogFormat("{0}: {1}", name, string.Join(", ", cartridges));
    }
}

// 결과
// AWP: "7.62x51mm NATO", ".308 Winchester", ".243 Winchester"
// PSG1: "7.62x51mm NATO"
```

#### 필드 배열에 사용할 수 있는 변환 메서드

개체를 생성하는데 여러 필드의 값이 필요한 형식에 사용할 수 있는 변환 메서드가 있습니다. 유니티 `Vector2`, `Vector3`, `Color`, `Color32` 형식에 대한 변환 메서드가 정의되어 있습니다.

예를 들어 `Vector2` 형식은 다음과 같이 변환할 수 있습니다.

```csharp
using UnityEngine;
using Macaron.Csv;

var str =
    "PositionX,PositionY\r\n" +
    "1,-1\r\n" +
    "-1,1\r\n";
var headerPolicy = CsvHeaderPolicies.FirstRecord();

using (var reader = CsvReader.Create(str, CsvReaderSettings.Default, headerPolicy))
{
    while (reader.Read())
    {
        var record = reader.Record;
        var position = record.Parse("PositionX", "PositionY").AsVector2();

        Debug.Log("Position: " + position);
    }
}

// 결과
// Position: (1.0, -1.0)
// Position: (-1.0, 1.0)
```
