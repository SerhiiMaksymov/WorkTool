namespace WorkTool.Core.XUnitTests.Modules.FileSystem.Views;

public class DiskUsageViewTests : AvaloniaFixture
{
    private readonly Mock<IDirectoryService> mockDirectoryService;
    private readonly IRandom<string> randomString;
    private readonly IRandom<ushort> randomUInt16;
    private readonly IRandom<ulong, Interval<ulong>> randomUInt64;
    private readonly QuantitiesInformation totalSize;
    private readonly QuantitiesInformation minSize;
    private readonly QuantitiesInformation maxSize;
    private readonly TestTaskCompletionSourceEnumerator taskCompletionSourceEnumerator;

    public DiskUsageViewTests()
    {
        taskCompletionSourceEnumerator = new TestTaskCompletionSourceEnumerator();
        totalSize = QuantitiesInformation.FromKiB(200);
        maxSize = QuantitiesInformation.FromKiB(10);
        minSize = 1ul;
        randomUInt64 = new RandomUInt64InInterval();
        randomUInt16 = new RandomUInt16(new RandomUInt16InInterval(), new Interval<ushort>(1, 5));
        randomString = RandomStringGuid.Digits;
        mockDirectoryService = new Mock<IDirectoryService>();
    }

    [Fact]
    public async Task Test()
    {
        SetupDirectoryServiceWithSize(totalSize);
        Injector.RegisterTransient(() => mockDirectoryService.Object);
        Injector.RegisterSingleton<ITaskCompletionSourceEnumerator>(taskCompletionSourceEnumerator);
        Injector.RegisterSingleton<IScheduler>(Scheduler);

        await Scheduler.WithAsync(async _ =>
        {
            var view = SetView<DiskUsageView>().ThrowIfNull();
            var viewModel = view.ViewModel.ThrowIfNull();
            viewModel.StartCommand.Execute();
            await taskCompletionSourceEnumerator.WaitGetCurrent.Task;
            ShowInfo(viewModel);
        });
    }

    private void ShowInfo(DiskUsageViewModel viewModel)
    {
        foreach (var dir in viewModel.Roots)
        {
            ShowInfo(dir, "");
        }
    }

    private void ShowInfo(DirectoryViewModel root, string offset)
    {
        Console.WriteLine($"{offset}{root.Directory?.ToPathString()}");

        foreach (var dir in root.Directories)
        {
            ShowInfo(dir, "-" + offset);
        }
    }

    private void SetupDirectoryServiceWithSize(QuantitiesInformation size)
    {
        var currentSize = size;

        mockDirectoryService
            .Setup(x => x.GetDirectories(It.IsAny<IDirectory>()))
            .Returns(() =>
            {
                if (currentSize == QuantitiesInformation.Zero)
                {
                    return Enumerable.Empty<IDirectory>();
                }

                var count = randomUInt16.GetRandom();
                var result = new IDirectory[count];

                for (var index = 0; index < count; index++)
                {
                    var path = randomString.GetRandom();
                    result[index] = new FileSystemDirectory(
                        path.ThrowIfNull(),
                        mockDirectoryService.Object
                    );
                }

                return result.AsEnumerable();
            });

        mockDirectoryService
            .Setup(x => x.GetFiles(It.IsAny<IDirectory>()))
            .Returns(() =>
            {
                if (currentSize == QuantitiesInformation.Zero)
                {
                    return Enumerable.Empty<IFile>();
                }

                var count = randomUInt16.GetRandom();
                var result = new IFile[count];

                for (var index = 0; index < count; index++)
                {
                    var interval = CreateInterval(minSize, maxSize, currentSize);
                    var fileSize = randomUInt64.GetRandom(interval);
                    result[index] = new FileSystemFile(fileSize);
                    currentSize -= fileSize;
                }

                return result.AsEnumerable();
            });
    }

    private Interval<ulong> CreateInterval(
        QuantitiesInformation min,
        QuantitiesInformation max,
        QuantitiesInformation current
    )
    {
        var minValue = QuantitiesInformation.MinMagnitude(min, current);
        var maxValue = QuantitiesInformation.MinMagnitude(max, current);
        var interval = new Interval<ulong>(minValue, maxValue);

        return interval;
    }
}
