require 'albacore'
require 'securerandom'
require 'fileutils'

desc "Build the entire v3qa"
solutions = ["Wonga.QA.Framework","Wonga.QA.Tests", "Wonga.QA.UiTests", "Wonga.QA.ServiceTests",
			"Wonga.QA.DataTests", "Wonga.QA.MigrationTests", "Wonga.QA.Tools"]
$bin_folder = 'bin'
$run_folder = 'run'
$src_folder = 'src'
$config_folder = File.join($run_folder, 'config')

task :build => :config do
	solutions.each do |solution|
		puts "src/**/#{solution}.sln"
		msb = MSBuild.new
		msb.properties :configuration =>  :Debug
		msb.solution = FileList.new("src/**/#{solution}.sln").first
		msb.execute
	end
end

exec :merge do |cmd|
	files = ['c:\dev\v3qa\bin\Wonga.QA.Tests.Meta.dll', 'c:\dev\v3qa\bin\Wonga.QA.Tests.WongaPay.dll']
	msbuild_tools_path = read_msbuild_property("MSBuildToolsPath")
	files_parameter = files.join(' ')
	cmd.command = 'ilmerge.exe'
    cmd.parameters = " /targetplatform:v4,#{msbuild_tools_path} "
	cmd.parameters += ' /out:bin\Wonga.QA.Tests.dll '
	cmd.parameters += files_parameter
    puts 'merged.'
end

task :config, [:test_target] do |t, args|
	args.with_defaults(:test_target => ENV['QAFTestTarget'])
	puts args.test_target
end

#creates an msbuild file, replaces the property you wanted and outputs it, then it captures the output
#and returns it back to the called.
def read_msbuild_property(property)
	temp_folder = 'bin\temp' #ENV['Temp']
	FileUtils.makedirs temp_folder
	build_file_name = SecureRandom.base64(6).gsub(/[$=+\/]/,65.+(rand(25)).chr)
	build_file_text = '<Project DefaultTargets="Default" ToolsVersion="4.0" xmlns="http://schemas.microsoft.com/developer/msbuild/2003">	<Target Name="Default"><Message Text="[[$(msbuildproperty)]]"/></Target></Project>'
	build_file_text = build_file_text.gsub(/msbuildproperty/, property)
	build_file_path = File.join(temp_folder, build_file_name)
	
	File.open build_file_path, "w" do |fl|
		fl.puts build_file_text
	end
	process = IO.popen("msbuild #{build_file_path} /nologo /v:normal")
	output = process.gets('150')
	File.delete build_file_path
	output.match(/\[\[(.*)\]\]/)[1]
end