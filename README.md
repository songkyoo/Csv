# Csv for Unity

CSV 데이터를 분석하여 레코드 단위의 문자열 목록으로 변환하는 유니티용 라이브러리입니다.

## 예제

### 문자열로 부터 읽기

```csharp
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

### Stream이나 TextReader로부터 읽기

```csharp
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

헤더 정보가 있다면 인덱스가 아닌 문자열을 사용하여 특정 열의 필드에 접근할 수 있습니다.

```csharp
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

또는 헤더와 같은 이름을 가지는 열거형을 사용할 수 있습니다.

```csharp
enum Header
{
    Manufacturer,
    Name,
    Cartridge
}

var str =
    "Manufacturer,Name,Cartridge\r\n" +
    "Heckler & Koch,PSG1,76x51mm NATO\r\n" +
    "Denel,NTW-20,20x82mm\r\n";

// 첫 번째 레코드를 헤더로 사용하고 헤더에 접근하기 위해 같은 이름을 가지는 열거형을 사용합니다.
var headerPolicy = CsvHeaderPolicies.FirstRecord<Header>();

// CSV 데이터가 헤더를 가지지 않는 경우 직접 입력할 수 있습니다.
// var headerPolicy = CsvHeaderPolicies.UserDefined<Header>(new[] { "Manufacturer", "Name", "Cartridge" });

// 헤더 정보가 없다면 열거형의 이름을 사용합니다. 순서는 열거형의 값으로 결정됩니다.
// var headerPolicy = CsvHeaderPolicies.UserDefined<Header>();

using (var reader = CsvReader.Create(str, CsvReaderSettings.Default, headerPolicy))
{
    while (reader.Read())
    {
        var record = reader.Record;
        Debug.LogFormat(
            "{0} {1} {2}",
            record.Get(Header.Manufacturer),
            record.Get(Header.Name),
            record.Get(Header.Cartridge));
    }
}

// 결과
// Heckler & Koch PSG1 76x51mm NATO
// Denel NTW-20 20x82mm
```

### CsvReaderSettings를 사용하여 CSV 형식을 지정

CSV 형식과 관련된 설정을 지정하여 `ICsvReader<T>` 개체를 생성할 수 있습니다.

값 | 형식 | 설명
-- | -- | --
FieldSeparator | char | 필드 구분자. `\r`, `\n`일 수 없습니다.
Quote | char? | 인용부호. `FieldSeparator`, `\r`, `\n`일 수 없습니다. `null`인 경우 인용부호에 대한 처리를 하지 않습니다.
Escape | char? | 이스케이프. `FieldSeparator`, `\r`, `\n`일 수 없습니다. `null`인 경우 이프케이프에 대한 처리를 하지 않으며, CSV 데이터가 이스케이프를 포함하고 있는 경우 올바르게 분석하지 못할 수 있습니다.
RecordTerminator | CsvRecordTerminator? | 레코드 구분자. `null`인 경우 첫 번째 레코드의 구분자를 사용합니다. `Quote`와 `RecordTerminator`가 모두 `null`인 경우 필드가 새줄 문자를 포함하고 있다면 올바르게 분석하지 못합니다.
TrimMode | CsvTrimMode | 인용되지 않은 필드의 전후 공백을 처리하는 방법.
NullValue | string | `null`이 아니라면 이 값과 동일한 값을 가지는 필드는 해당 값 대신 `null` 값을 가지게 됩니다. 값 비교는 `TrimMode`가 적용된 후에 이루어지며 서수 비교를 수행합니다.

CsvReaderSettings는 자주 사용하는 형식에 대해 미리 정의된 정적 필드를 가지고 있습니다.

필드 | 설명
-- | --
Default | 기본값. RFC4180 형식을 따르나 RecordTerminator 값이 `null`입니다.
RFC4180 | RFC4180 형식. 마이크로소프트 엑셀, 구글 스프레드시트 등 일반적인 스프레드시트 프로그램에서 필드 구분자를 `,`로 설정하여 CSV를 생성한 경우 사용할 수 있습니다.
MySQL | `SELECT ... INTO OUTFILE` 쿼리의 기본 옵션으로 생성된 CSV에 대한 형식.

CsvReaderSettings는 구조체 형식이기 때문에 기존값을 복사 후 수정할 수 있습니다.

```csharp
var settings = CsvReaderSettings.Default;
settings.FieldSeparator = '\t'; // 기존값에 영향을 주지 않습니다.
```
