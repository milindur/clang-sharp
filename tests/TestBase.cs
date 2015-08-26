﻿using System.IO;
using ClangSharp;
using NUnit.Framework;
using tests.Properties;

namespace tests {
    [SetUpFixture]
    public class TestBase {
        protected static string FakeClassCpp = Path.Combine(Path.GetTempPath(), "fake-class.cpp");
        protected static string FakeClassH = Path.Combine(Path.GetTempPath(), "fake-class.h");
        protected static string OpaqueClassH = Path.Combine(Path.GetTempPath(), "opaque-class.h");
        protected static string MainCpp = Path.Combine(Path.GetTempPath(), "fake-main.cpp");
        protected static string KitchenSinkCpp = Path.Combine(Path.GetTempPath(), "kitchen-sink.cpp");

        protected static Index Index;
        protected static TranslationUnit Main;
        protected static TranslationUnit Class;
        protected static TranslationUnit KitchenSink;

        [SetUp]
        public static void CreateTestFiles() {
            System.IO.File.WriteAllText(FakeClassCpp, Resources.fake_class_cpp);
            System.IO.File.WriteAllText(FakeClassH, Resources.fake_class_h);
            System.IO.File.WriteAllText(OpaqueClassH, Resources.opaque_class_h);
            System.IO.File.WriteAllText(MainCpp, Resources.fake_main_cpp);
            System.IO.File.WriteAllText(KitchenSinkCpp, Resources.kitchen_sink);
            var args = new[] { Options.Weverything };
            var unsavedFiles = new UnsavedFile[] { };
            var options = TranslationUnitFlags.IncludeBriefCommentsInCodeCompletion | TranslationUnitFlags.DetailedPreprocessingRecord;
            Index = new Index();
            Main = Index.CreateTranslationUnit(MainCpp, args, unsavedFiles, options);
            Class = Index.CreateTranslationUnit(FakeClassCpp, args, unsavedFiles, options);
            KitchenSink = Index.CreateTranslationUnit(KitchenSinkCpp, args, unsavedFiles, options);
        }

        [TearDown]
        public static void DeleteTestFiles() {
            foreach (string file in new[] { FakeClassCpp, FakeClassH, OpaqueClassH, MainCpp, KitchenSinkCpp }) {
                System.IO.File.Delete(file);
            }
            Main.Dispose();
            Index.Dispose();
        }
    }
}
